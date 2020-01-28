using UnityEngine;
using System.Collections;

public class Iron : MonoBehaviour {
    public Scrubber scrubber;

    private Vector2 UV = Vector2.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionStay(Collision col)
    {
        var scrubbable = col.collider.gameObject.GetComponentInChildren<Scrubbable>();
        if (scrubbable == null)
            return;

        // we can only find UV coords of collisions with MeshColliders, not other Collider types
        if (col.collider is MeshCollider)
        {
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
                Debug.DrawRay(centroid + col.contacts[0].normal, -col.contacts[0].normal, Color.red);
                if (!col.contacts[0].otherCollider.Raycast(new Ray(centroid + col.contacts[0].normal, -col.contacts[0].normal), out hit, 5.5f))
                {
                    return;
                }

                if (hit.collider.gameObject != col.collider.gameObject)
                    return;

                Vector2 uv = hit.textureCoord;
                UV = uv;
                scrubber.Scrub(scrubbable, uv);
            }
        }
    }
}
