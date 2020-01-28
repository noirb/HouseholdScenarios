using UnityEngine;
using System.Collections;

/// <summary>
/// Continually updates an object's transform to follow another
/// </summary>
public class FollowTransform : MonoBehaviour {
    [Tooltip("Transform to follow")]
    public Transform target = null;
    [Tooltip("Offset from target to maintain")]
    public Vector3 initial_offset;

	// Use this for initialization
	void Start () {
        initial_offset = target.position - this.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        this.transform.position = target.position - initial_offset;
	}
}
