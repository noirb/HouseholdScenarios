using UnityEngine;
using System.Collections.Generic;

public static class LogHacker {
    /// <summary>
    /// Reads log data from file
    /// </summary>
    /// <param name="filename">File to read from</param>
    /// <returns></returns>
    private static ScenarioLog load(string filename)
    {
        filename = filename.Replace("\"", "");
        ScenarioLog log = new ScenarioLog();

        try
        {
            Debug.Log("Reading input log: " + filename);
            string log_data = System.IO.File.ReadAllText(filename);
            log = JsonUtility.FromJson<ScenarioLog>(log_data);
        }
        catch (System.IO.FileNotFoundException ex)
        {
            Debug.LogWarning("Could not find log file: '" + filename + "'\n" + ex.Message);
            return null;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("An exception was thrown while trying to read log file '" + filename + "':\n" + ex.Message);
            return null;
        }

        return log;
    }

    /// <summary>
    /// Saves a log to a file
    /// </summary>
    /// <param name="filename">File to write log to</param>
    /// <param name="log">Log to save</param>
    private static void save(string filename, ref ScenarioLog log)
    {
        filename = filename.Replace("\"", "");

        Debug.Log("JSON-ifying log...");
        string total_log = JsonUtility.ToJson(log, true);

        Debug.Log("Writing results to output file: " + filename);
        string dirName = filename.Substring(0, filename.LastIndexOf('\\'));
        if (!System.IO.Directory.Exists(dirName))
        {
            Debug.Log("Creating directory: " + dirName);
            System.IO.Directory.CreateDirectory(dirName);
        }

        System.IO.File.WriteAllText(filename, total_log);

        Debug.Log("Results saved!");
    }

    /// <summary>
    /// Replaces all velocity values in a log file with estimates based on position data
    /// </summary>
    /// <param name="log">The log to hack</param>
    public static void HackVelocity(ref ScenarioLog log)
    {
        Dictionary<string, Vector3> prev_poses = new Dictionary<string, Vector3>();
        float prev_time = 0.0f;

        for (int i = 0; i < log.Count; i++)
        {
            float time_diff = 1.0f / (log.log[i].time - prev_time);
            foreach (var entry in log.log[i].logstep)
            {
                if (prev_poses.ContainsKey(entry.name))
                {
                    // figure new velocity
                    Vector3 pos_diff = new Vector3(entry.position[0], entry.position[1], entry.position[2]) - prev_poses[entry.name];
                    
                    Vector3 velocity = pos_diff * time_diff;
                    entry.velocity[0] = velocity.x;
                    entry.velocity[1] = velocity.y;
                    entry.velocity[2] = velocity.z;
                }

                prev_poses[entry.name] = new Vector3(entry.position[0], entry.position[1], entry.position[2]);
            }

            prev_time = log.log[i].time;
        }
    }

    /// <summary>
    /// Replaces all velocity values in a log file with estimates based on position data
    /// </summary>
    /// <param name="inFile">File to read log data from</param>
    /// <param name="outFile">File to write results to</param>
    public static void HackVelocity(string inFile, string outFile)
    {
        // clear any old log data
        ScenarioLog log = load(inFile);

        // in case of errors reading inFile
        if (log == null)
            return;

        Debug.Log("Hacking log velocities...");
        HackVelocity(ref log);

        save(outFile, ref log);
    }

    /// <summary>
    /// Removes all entries for an object from a given log file
    /// </summary>
    /// <param name="objectName">Object to prune from log</param>
    /// <param name="log">Log to remove object from</param>
    public static void PruneObject(string objectName, ref ScenarioLog log)
    {
        foreach (var logEntry in log.log)
        {
            List<int> delIndices = new List<int>();

            for (int i = 0; i < logEntry.logstep.Count; i++)
            {
                if (logEntry.logstep[i].name == objectName)
                {
                    delIndices.Add(i);
                }
            }

            if (delIndices.Count > 0)
            {
                for (int i = delIndices.Count-1; i >= 0; i--)
                {
                    logEntry.logstep.RemoveAt(delIndices[i]);
                }
            }
        }
    }

    /// <summary>
    /// Removes all entries for an object from a given log file
    /// </summary>
    /// <param name="objectName">Object to prune from log</param>
    /// <param name="inFile">File to read log from</param>
    /// <param name="outFile">File to write results to</param>
    public static void PruneObject(string objectName, string inFile, string outFile)
    {
        ScenarioLog log = load(inFile);

        // in case of errors reading inFile
        if (log == null)
            return;

        Debug.Log("Hacking '" + objectName + "' out of log...");
        PruneObject(objectName, ref log);

        save(outFile, ref log);
    }
}
