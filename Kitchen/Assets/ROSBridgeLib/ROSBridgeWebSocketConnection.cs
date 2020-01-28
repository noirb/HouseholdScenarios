using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System;
using System.Text.RegularExpressions;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using UnityEngine;


namespace ROSBridgeLib
{
    public delegate void OnConnectionStateChanged();
    public delegate void ROSMessageCallback<in Tmsg>(Tmsg msg);
    public delegate bool ROSServiceCallback<in Targs, Tresp>(Targs args, out Tresp resp);
    delegate void MessageCallback(ROSMessage msg); // internal; used to wrap ROSMessageCallbacks
    delegate bool ServiceCallback(ServiceArgs args, out ServiceResponse response); // internal; used to wrap ROSServiceCallbacks

    [System.Serializable]
    public class Topper
    {
        public string op;
        public string topic;

        public Topper(string jsonString)
        {

            Topper message = JsonUtility.FromJson<Topper>(jsonString);
            op = message.op;
            topic = message.topic;
        }
    }

    [System.Serializable]
    public class ServiceHeader
    {
        public string op;
        public string service;
        public string id;

        public ServiceHeader(string jsonstring)
        {
            JsonUtility.FromJsonOverwrite(jsonstring, this);
        }
    }

    public class ROSBridgeWebSocketConnection
    {
        /// <summary>
        /// A queue with a limited maximum size. If an object added to the queue causes the queue
        /// to go over the specified maximum size, it will automatically dequeue the oldest entry.
        /// A maximum size of 0 implies an unrestricted queue (making this equivalent to Queue<T>)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class RenderQueue<T> : Queue<T>
        {
            // Maximum size of queue. Unrestricted if zero.
            private uint _maxSize = 0;

            public RenderQueue(uint maxSize)
            {
                _maxSize = maxSize;
            }

            public new void Enqueue(T obj)
            {
                base.Enqueue(obj);
                if (Count > _maxSize && _maxSize > 0)
                {
                    base.Dequeue();
                }
            }

        }

        private struct MessageTask
        {
            private ROSBridgeSubscriber _subscriber;
            private ROSMessage _msg;

            public MessageTask(ROSBridgeSubscriber subscriber, ROSMessage msg)
            {
                _subscriber = subscriber;
                _msg = msg;
            }

            public ROSBridgeSubscriber getSubscriber()
            {
                return _subscriber;
            }

            public ROSMessage getMsg()
            {
                return _msg;
            }
        };

        private struct PublishTask
        {
            private ROSBridgePublisher _publisher;
            private ROSMessage _msg;

            public PublishTask(ROSBridgePublisher publisher, ROSMessage msg)
            {
                _publisher = publisher;
                _msg = msg;
            }

            public ROSBridgePublisher publisher
            {
                get { return _publisher; }
            }

            public string message
            {
                get { return _publisher.ToMessage(_msg); }
            }
        }

        private class ServiceTask
        {
            private ROSBridgeServiceProvider _service;
            private ServiceArgs _request;
            private ServiceResponse _response;
            private string _id;

            public ServiceTask(ROSBridgeServiceProvider service, ServiceArgs request, string id)
            {
                _service = service;
                _request = request;
                _id = id;
            }

            public ROSBridgeServiceProvider Service
            {
                get { return _service; }
            }

            public ServiceArgs Request
            {
                get { return _request; }
            }

            public ServiceResponse Response
            {
                get { return _response; }
                set { _response = value; }
            }

            public string id
            {
                get { return _id; }
            }
        }

        private bool _connected = false;
        private string _host;
        private int _port;
        private WebSocket _ws;
        private System.Threading.Thread _recvThread;  // thread for reading data from the network
        private Dictionary<ROSBridgeSubscriber, MessageCallback> _subscribers; // our subscribers
        private List<ROSBridgePublisher> _publishers; // our publishers
        private Dictionary<ROSBridgeServiceProvider, ServiceCallback> _serviceServers; // service providers
        private Dictionary<string, RenderQueue<MessageTask>> _msgQueue = new Dictionary<string, RenderQueue<MessageTask>>();
        private Dictionary<string, RenderQueue<ServiceTask>> _svcQueue = new Dictionary<string, RenderQueue<ServiceTask>>();
        private Queue<PublishTask> _pubQueue = new Queue<PublishTask>();

        private object _readLock = new object();

        public OnConnectionStateChanged onConnectionSuccess;
        public OnConnectionStateChanged onConnectionFailure;

        public bool connected
        {
            get { return _connected; }
        }

        public ROSBridgeWebSocketConnection(string host, int port) : this(host, port, 1) // default to msg queue size of 1 if unspecified
        {
        }

        public ROSBridgeWebSocketConnection(string host, int port, uint max_msg_queue_size)
        {
            _host = host;
            _port = port;
            _recvThread = null;
            _publishers = new List<ROSBridgePublisher>();
            _subscribers = new Dictionary<ROSBridgeSubscriber, MessageCallback>();
            _serviceServers = new Dictionary<ROSBridgeServiceProvider, ServiceCallback>();
        }

        /// <summary>
        /// Add a publisher to this connection. There can be many publishers.
        /// </summary>
        /// <typeparam name="Tpub">Publisher type to advertise</typeparam>
        /// <param name="topic">Topic to advertise on</param>
        /// <returns>A publisher which can be used to broadcast data on the given topic</returns>
        public ROSBridgePublisher<Tmsg> Advertise<Tmsg>(string topic) where Tmsg : ROSMessage
        {

            ROSBridgePublisher<Tmsg> pub = (ROSBridgePublisher<Tmsg>)Activator.CreateInstance(typeof(ROSBridgePublisher<Tmsg>), new object[] { topic });
            pub.SetBridgeConnection(this);

            _publishers.Add(pub);

            if (connected)
            {
                _ws.Send(ROSBridgeMsg.AdvertiseTopic(pub.topic, pub.type));
            }

            return pub;
        }

        /// <summary>
        /// Remove a publisher from this connection
        /// </summary>
        /// <param name="pub"></param>
        public void Unadvertise(ROSBridgePublisher pub)
        {
            if (connected)
            {
                _ws.Send(ROSBridgeMsg.UnAdvertiseTopic(pub.topic));
            }

            _publishers.Remove(pub);
        }

        /// <summary>
        /// Add a subscriber callback to this connection. There can be many subscribers.
        /// </summary>
        /// <typeparam name="Tmsg">Message type used in the callback</typeparam>
        /// <param name="sub">Subscriber</param>
        /// <param name="callback">Method to call when a message matching the given subscriber is received</param>
        public ROSBridgeSubscriber<Tmsg> Subscribe<Tmsg>(string topic, ROSMessageCallback<Tmsg> callback, uint queueSize = 0) where Tmsg : ROSBridgeLib.ROSMessage, new()
        {
            MessageCallback CB = (ROSMessage msg) =>
            {
                Tmsg message = msg as Tmsg;
                callback(message);
            };

            var getMessageType = typeof(Tmsg).GetMethod("GetMessageType");
            if (getMessageType == null)
            {
                Debug.LogError("Could not retrieve method GetMessageType() from " + typeof(Tmsg).ToString());
                return null;
            }
            string messageType = (string)getMessageType.Invoke(null, null);
            if (messageType == null)
            {
                Debug.LogError("Could not retrieve valid message type from " + typeof(Tmsg).ToString());
                return null;
            }

            ROSBridgeSubscriber<Tmsg> sub = new ROSBridgeSubscriber<Tmsg>(topic, messageType);
            
            _subscribers.Add(sub, CB);
            _msgQueue.Add(sub.topic, new RenderQueue<MessageTask>(queueSize));

            if (connected)
            {
                _ws.Send(ROSBridgeMsg.Subscribe(sub.topic, sub.type));
            }

            return sub;
        }
        

        /// <summary>
        /// Remove a subscriber callback from this connection.
        /// </summary>
        /// <param name="sub"></param>
        public void Unsubscribe(ROSBridgeSubscriber sub)
        {
            if (sub == null)
            {
                return;
            }

            _subscribers.Remove(sub);
            _msgQueue.Remove(sub.topic);

            if (connected)
            {
                _ws.Send(ROSBridgeMsg.UnSubscribe(sub.topic));
            }
        }

        /// <summary>
        /// Add a Service server to this connection. There can be many servers, but each service should only have one.
        /// </summary>
        /// <typeparam name="Tsrv">ServiceProvider type</typeparam>
        /// <typeparam name="Treq">Message type containing parameters for this service</typeparam>
        /// <typeparam name="Tres">Message type containing response data returned by this service</typeparam>
        /// <param name="srv">The service to advertise</param>
        /// <param name="callback">Method to invoke when the service is called</param>
        public ROSBridgeServiceProvider<Treq> Advertise<Tsrv, Treq, Tres>(string service, ROSServiceCallback<Treq, Tres> callback) where Tsrv : ROSBridgeServiceProvider<Treq> where Treq : ServiceArgs where Tres : ServiceResponse, new()
        {
            ServiceCallback CB = (ServiceArgs args, out ServiceResponse response) =>
            {
                Treq request = (Treq)args;
                Tres res = new Tres();
                bool success = callback(request, out res);
                response = res;
                return success;
            };

            Tsrv srv = (Tsrv)Activator.CreateInstance(typeof(Tsrv), new object[] { service } );
            _serviceServers.Add(srv, CB);
            _svcQueue.Add(srv.topic, new RenderQueue<ServiceTask>(0));

            if (connected)
            {
                _ws.Send(ROSBridgeMsg.AdvertiseService(srv.topic, srv.type));
            }

            return srv;
        }

        /// <summary>
        /// Remove a Service server from this connection
        /// </summary>
        /// <param name="srv"></param>
        public void Unadvertise(ROSBridgeServiceProvider srv)
        {
            if (connected)
            {
                _ws.Send(ROSBridgeMsg.UnadvertiseService(srv.topic));
            }
            _serviceServers.Remove(srv);
        }

        /// <summary>
        /// Connect to the remote ros environment.
        /// </summary>
        public void Connect()
        {
            if (connected)
                return;

            _recvThread = new System.Threading.Thread(Run);
            _recvThread.Start();
        }

        /// <summary>
        /// Disconnect from the remote ros environment.
        /// </summary>
        public void Disconnect()
        {
            if (!connected)
                return;

            _recvThread.Abort();
            foreach (var sub in _subscribers)
            {
                _ws.Send(ROSBridgeMsg.UnSubscribe(sub.Key.topic));
            }
            foreach (var p in _publishers)
            {
                _ws.Send(ROSBridgeMsg.UnAdvertiseTopic(p.topic));
            }
            foreach (var srv in _serviceServers)
            {
                _ws.Send(ROSBridgeMsg.UnadvertiseService(srv.Key.topic));
            }

            _ws.Close();
            _msgQueue.Clear();

            _connected = false;
        }

        private void Run()
        {
            _ws = new WebSocket(_host + ":" + _port);
            _ws.OnMessage += (sender, e) => this.OnMessage(e.Data);
            _ws.OnError += _ws_OnError;
            _ws.OnOpen += _ws_OnOpen;
            _ws.Connect();

            while (!connected)
            {
                Thread.Sleep(10);
            }
            foreach (var sub in _subscribers)
            {
                _ws.Send(ROSBridgeMsg.Subscribe(sub.Key.topic, sub.Key.type));
                Debug.Log("Sending: " + ROSBridgeMsg.Subscribe(sub.Key.topic, sub.Key.type));
            }
            foreach (var p in _publishers)
            {
                _ws.Send(ROSBridgeMsg.AdvertiseTopic(p.topic, p.type));
                Debug.Log("Sending " + ROSBridgeMsg.AdvertiseTopic(p.topic, p.type));
            }
            foreach (var srv in _serviceServers)
            {
                _ws.Send(ROSBridgeMsg.AdvertiseService(srv.Key.topic, srv.Key.type));
                Debug.Log("Sending: " + ROSBridgeMsg.AdvertiseService(srv.Key.topic, srv.Key.type));
            }

            while (_connected)
            {
                Thread.Sleep(10);
            }
        }

        private void _ws_OnOpen(object sender, EventArgs e)
        {
            Debug.Log("Websocket open!");
            _connected = true;

            if (onConnectionSuccess != null)
                onConnectionSuccess();
        }

        private void _ws_OnError(object sender, ErrorEventArgs e)
        {
            Debug.LogError(e.Message);
            _connected = false;

            if (onConnectionFailure != null)
                onConnectionFailure();
        }

        private void OnMessage(string s)
        {
            if ((s != null) && !s.Equals(""))
            {
                Topper mms = new Topper(s);

                string op = mms.op;

                if ("publish".Equals(op))
                {
                    string topic = mms.topic;
                    string msg_params = "";

                    // if we have message parameters, parse them
                    Match m = Regex.Match(s, @"""msg""\s*:\s*({.*}),");
                    ROSMessage msg = null;
                    if (m.Success)
                    {
                        msg_params = m.Groups[1].Value;
                    }

                    foreach (var sub in _subscribers)
                    {
                        // only consider subscribers with a matching topic
                        if (topic != sub.Key.topic)
                            continue;

                        msg = sub.Key.ParseMessage(msg_params);
                        MessageTask newTask = new MessageTask(sub.Key, msg);
                        lock (_readLock)
                        {
                            _msgQueue[topic].Enqueue(newTask);
                        }

                    }
                }
                else if (Regex.IsMatch(s, @"""op""\s*:\s*""call_service""")) // op is call_service
                {
                    ServiceHeader hdr = new ServiceHeader(s);
                    foreach (var srv in _serviceServers)
                    {
                        if (srv.Key.topic == hdr.service)
                        {
                            ServiceArgs args = null;
                            ServiceResponse response = null;
                            // if we have args, parse them (args are optional on services, though)
                            Match m = Regex.Match(s, @"""args""\s*:\s*({.*}),");
                            if (m.Success)
                            {
                                args = srv.Key.ParseRequest(m.Groups[1].Value);
                            }

                            // add service request to queue, to be processed later in Render()
                            lock(_readLock)
                            {
                                _svcQueue[srv.Key.topic].Enqueue(new ServiceTask(srv.Key, args, hdr.id));
                            }

                            break; // there should only be one server for each service
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Unhandled message:\n" + s);
                }
            }
            else
            {
                Debug.Log("Got an empty message from the web socket");
            }
        }

        /// <summary>
        /// Should be called at least once each frame. Calls any available callbacks for received messages.
        /// Note: MUST be called from Unity's main thread!
        /// </summary>
        public void Render()
        {
            float start = Time.realtimeSinceStartup;    // time at start of this frame
            float max_dur = 0.5f * Time.fixedDeltaTime; // max time we want to spend working
            float dur = 0.0f;                           // time spent so far processing messages

            while (dur < max_dur)
            {
                // get queued work to do
                List<MessageTask> msg_tasks = MessagePump();
                List<ServiceTask> svc_tasks = ServicePump();

                // bail if we have no work to do
                if (msg_tasks.Count == 0 && svc_tasks.Count == 0)
                    break;

                // call all msg subsriber callbacks
                foreach (var t in msg_tasks)
                {
                    _subscribers[t.getSubscriber()](t.getMsg());
                }

                // call all svc handlers
                foreach (var svc in svc_tasks)
                {
                    ServiceResponse response = null;

                    // invoke service handler
                    bool success = _serviceServers[svc.Service](svc.Request, out response);

                    Debug.Log("Sending service response: \n" + ROSBridgeMsg.ServiceResponse(success, svc.Service.topic, svc.id, JsonUtility.ToJson(response)));
                    // send response
                    _ws.SendAsync(ROSBridgeMsg.ServiceResponse(success, svc.Service.topic, svc.id, JsonUtility.ToJson(response)), null);
                }

                dur = Time.realtimeSinceStartup - start;
            }
        }

        /// <summary>
        /// Pulls one message from each queue for processing
        /// </summary>
        /// <returns>A list of queued messages</returns>
        private List<MessageTask> MessagePump()
        {
            List<MessageTask> tasks = new List<MessageTask>();
            lock (_readLock)
            {
                foreach (var item in _msgQueue)
                {
                    // peel one entry from each queue to process on this frame
                    if (item.Value.Count > 0)
                    {
                        tasks.Add(item.Value.Dequeue());
                    }
                }
            }
            return tasks;
        }

        /// <summary>
        /// Pulls one message from each service queue for processing
        /// </summary>
        /// <returns>A list of queued service requests</returns>
        private List<ServiceTask> ServicePump()
        {
            List<ServiceTask> tasks = new List<ServiceTask>();
            lock (_readLock)
            {
                foreach (var item in _svcQueue)
                {
                    // peel one entry from each queue to process on this frame
                    if (item.Value.Count > 0)
                    {
                        tasks.Add(item.Value.Dequeue());
                    }
                }
            }
            return tasks;
        }

        /// <summary>
        /// Publish a message to be sent to the ROS environment. Note: You must Advertise() before you can Publish().
        /// </summary>
        /// <param name="publisher">Publisher associated with the topic to publish to</param>
        /// <param name="msg">Message to publish</param>
        public void Publish(ROSBridgePublisher publisher, ROSMessage msg)
        {
            if (_ws != null && connected)
            {
                _ws.Send(publisher.ToMessage(msg));
            }
            else
            {
                Debug.LogWarning("Could not publish message! No current connection to ROSBridge...");
            }
        }

    }
}
