using UnityEngine;
using System.Collections;
using NewtonVR;

public class Shirt : NewtonVR.NVRInteractableItem {
    [Tooltip("GameObject holding actual mesh, and mesh collider for scrubbing")]
    public MeshCollider realShirt;
    [Tooltip("GameObject holding (convex) collision mesh for shirt during regular physics")]
    public Collider shirtCollider;

    bool onBoard = false;

    protected override void Awake()
    {
        realShirt.enabled = false; // disable to start
        base.Awake();
    }

    public override void BeginInteraction(NVRHand hand)
    {
        realShirt.enabled = false;
        shirtCollider.enabled = true;
        base.BeginInteraction(hand);
    }

    public override void EndInteraction()
    {
        base.EndInteraction();

        if (onBoard)
        {
            Rigidbody.isKinematic = true;
            realShirt.enabled = true;
            shirtCollider.enabled = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "ironing_board")
        {
            onBoard = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "ironing_board")
        {
            onBoard = false;
        }
    }
}
