using UnityEngine;
using System.Collections;

public class HeadIK : MonoBehaviour
{
    public bool enableIK = true;
    public Transform lookTarget;
    public Transform head;
    public float head_height = 0.45f;
    public float body_rotation_speed = 2.0f;

    [Space]

    public float lookWeight = 1.0f;
    public float bodyWeight = 0.25f;
    public float headWeight = 0.9f;
    public float eyesWeight = 1.0f;
    public float clampWeight = 1.0f;

    protected Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // drag body along with head
        transform.position = new Vector3(head.position.x, head.position.y - head_height, head.position.z);

        // rotate body towards the direction the head is facing
        Quaternion new_rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, head.rotation.eulerAngles.y, 0), body_rotation_speed);
        transform.rotation = new_rot;
    }

    void OnAnimatorIK()
    {
        if (enableIK)
        {
            animator.SetLookAtWeight(lookWeight, bodyWeight, headWeight, eyesWeight, clampWeight);

            if (lookTarget != null)
            {
                animator.SetLookAtPosition(lookTarget.position);
            }
        }
    }
}
