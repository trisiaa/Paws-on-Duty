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
        target.y = transform.position.y;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        transform.LookAt(player);
    }
}