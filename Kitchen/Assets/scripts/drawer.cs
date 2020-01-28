using UnityEngine;
using System.Collections;


public class drawer : MonoBehaviour {
    bool first = true;

    void FixedUpdate()
    {
        if (first)
        {
            first = false;
            // auto-close drawer on startup
            Vector3 closed_pos = this.transform.parent.Find("closed_target").position;
            Vector3 close_dir = closed_pos - this.transform.position;
            this.gameObject.GetComponent<Rigidbody>().AddForce(close_dir.normalized * 5.0f, ForceMode.Acceleration);
        }
    }
}
