using UnityEngine;
using System.Collections;

/// <summary>
/// An object capable to wetting and washing other objects
/// </summary>
public class Sponge : PropertyActionProvider {
    [Tooltip("Whether the sponge is currently wet")]
    public bool wet;
    [Tooltip("Must be defined in order to clean surfaces")]
    public Scrubber scrubber;

    // Use this for initialization
    void Start() {

        if (wet)
        {
            AddAction(PropertyAction.MakeWet);
            AddAction(PropertyAction.MakeClean);
        }
    }

    public override string GetPropertyStatus()
    {
        if (wet)
        {
            return "Wetness:Wet";
        }
        else
        {
            return "Wetness:Dry";
        }
    }

    public void MakeWet()
    {
        if (!wet)
        {
            wet = true;
            AddAction(PropertyAction.MakeWet);
            AddAction(PropertyAction.MakeClean);
            this.GetComponent<MeshRenderer>().material.color = new Color(0.65f, 0.42f, 0.02f);
            ScenarioLogManager.Instance.LogEvent(this.gameObject, "PropertyChanged", "Wetness:Wet");
        }
    }

    public void MakeDry()
    {
        if (wet)
        {
            wet = false;
            RemoveAction(PropertyAction.MakeWet);
            RemoveAction(PropertyAction.MakeClean);
            this.GetComponent<MeshRenderer>().material.color = new Color(0.84f, 0.75f, 0.2f);
            ScenarioLogManager.Instance.LogEvent(this.gameObject, "PropertyChanged", "Wetness:Dry");
        }
    }

    void OnParticleCollision(GameObject other)
    {
        var actionProvider = other.GetComponent<PropertyActionProvider>();
        if (actionProvider != null)
        {
            var actions = actionProvider.GetActions();
            foreach (var action in actions)
            {
                switch (action)
                {
                    case PropertyAction.MakeWet:
                        MakeWet();
                        break;
                    case PropertyAction.MakeClean:
                        break;
                    case PropertyAction.MakeDry:
                        MakeDry();
                        break;
                    case PropertyAction.MakeDirty:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (!wet)
            return;

        // we can only find UV coords of collisions with MeshColliders, not other Collider types
        if (col.collider is MeshCollider)
        {
            var scrubbable = col.gameObject.GetComponent<Scrubbable>();
            if (scrubbable == null)
                return;

            var tex = scrubbable.GetMask();
            if (tex != null)
            {
                Vector3 centroid = Vector3.zero;
                foreach (var contact in col.contacts)
                {
                    centroid += contact.point;
                }
                centroid /= (float)col.contacts.Length;

                RaycastHit hit;
                if (!col.contacts[0].otherCollider.Raycast(new Ray(centroid + col.contacts[0].normal, -col.contacts[0].normal), out hit, 5.5f))
                {
                    return;
                }

                if (hit.collider.gameObject != col.gameObject)
                    return;

                Vector2 uv = hit.textureCoord;

                scrubber.Scrub(scrubbable, uv);
            }
        }
    }
}
