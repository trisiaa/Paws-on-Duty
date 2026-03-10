using UnityEngine;

public class MayaFollow : MonoBehaviour
{
    public Transform player;

    public float DN = 1.5f;
    public float DP = 2.5f;
    public float speed = 2f;

    enum State
    {
        Walk,
        Stop
    }

    State currentState = State.Stop;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if(currentState == State.Walk)
        {
            if(distance < DN)
            {
                currentState = State.Stop;
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

    void OnCollisionEnter(Collision collision)
    {
        currentState = State.Stop;
    }
}