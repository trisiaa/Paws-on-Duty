using UnityEngine;

public class MayaFollow : MonoBehaviour
{
    public Transform player;

    public float DN = 1.5f;
    public float DP = 2.5f;
    public float speed = 2f;

    Animator anim;

    enum State
    {
        Walk,
        Stop
    }

    State currentState = State.Stop;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if(currentState == State.Walk)
        {
            if(distance < DN)
            {
                currentState = State.Stop;
                anim.SetBool("isWalking", false);
            }
            else
            {
                MoveToPlayer();
            }
        }

        if(currentState == State.Stop)
        {
            if(distance > DP)
            {
                currentState = State.Walk;
                anim.SetBool("isWalking", true);
            }
        }
    }

    void MoveToPlayer()
    {
        Vector3 target = player.position;
        target.y = transform.position.y; // Keep her on the ground

        // 1. Move smoothly
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        // 2. Rotate smoothly instead of snapping with LookAt
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }
}