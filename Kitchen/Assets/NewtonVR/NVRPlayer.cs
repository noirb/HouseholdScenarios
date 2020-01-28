﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NewtonVR
{
    public class NVRPlayer : MonoBehaviour
    {
        public bool PhysicalHands = false;
        public bool MakeControllerInvisibleOnInteraction = false;
        public int VelocityHistorySteps = 3;

        [Space]

        public NVRHead Head;
        public NVRHand LeftHand;
        public NVRHand RightHand;

        [HideInInspector]
        public NVRHand[] Hands;

        private Dictionary<Collider, NVRHand> ColliderToHandMapping;

        [Space]

        public bool DEBUGDropFrames = false;
        public int DEBUGSleepPerFrame = 13;

        NVRPlayer()
        {
            ColliderToHandMapping = new Dictionary<Collider, NVRHand>();
        }

        private void Awake()
        {
            NVRInteractables.Initialize();

            if (Head == null)
            {
                Head = this.GetComponentInChildren<NVRHead>();
            }

            if (LeftHand == null || RightHand == null)
            {
                Debug.LogError("[FATAL ERROR] Please set the left and right hand to a nvrhands.");
            }

            if (Hands == null || Hands.Length == 0)
            {
                Hands = new NVRHand[] { LeftHand, RightHand };
            }

            LeftHand.player = this;
            RightHand.player = this;
        }

        public void RegisterHand(NVRHand hand)
        {
            Collider[] colliders = hand.GetComponentsInChildren<Collider>();

            for (int index = 0; index < colliders.Length; index++)
            {
                if (ColliderToHandMapping.ContainsKey(colliders[index]) == false)
                {
                    ColliderToHandMapping.Add(colliders[index], hand);
                }
            }
        }

        public NVRHand GetHand(Collider collider)
        {
            return ColliderToHandMapping[collider];
        }

        public void DeregisterInteractable(NVRInteractable interactable)
        {
            for (int index = 0; index < Hands.Length; index++)
            {
                Hands[index].DeregisterInteractable(interactable);
            }
        }

        private void Update()
        {
            if (DEBUGDropFrames == true)
            {
                System.Threading.Thread.Sleep(DEBUGSleepPerFrame);
            }
        }
    }
}