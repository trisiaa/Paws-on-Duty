using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
public CharacterController controller;
public float speed = 5f;


public Transform maya;
public float DP = 2.5f;

public Transform corridor;

Animator anim;

void Start()
{
    anim = GetComponentInChildren<Animator>();
}

void Update()
{
    float distance = Vector3.Distance(transform.position, maya.position);

    float currentSpeed = speed;

    if(distance > DP)
    {
        currentSpeed = speed * 0.2f;
    }

    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");

    Vector3 move = new Vector3(h,0,v);

    controller.Move(move * currentSpeed * Time.deltaTime);

    if(move.magnitude > 0.05f)
    {
        anim.SetBool("isWalking", true);

        transform.forward = Vector3.Lerp(
            transform.forward,
            move,
            10f * Time.deltaTime
        );
    }
    else
    {
        anim.SetBool("isWalking", false);
    }

    // BATASI PLAYER DI DALAM WALKCORRIDOR
    if(corridor != null)
    {
        Vector3 pos = transform.position;

        float halfWidth = corridor.localScale.x / 2f;
        float halfLength = corridor.localScale.z / 2f;

        pos.x = Mathf.Clamp(
            pos.x,
            corridor.position.x - halfWidth,
            corridor.position.x + halfWidth
        );

        pos.z = Mathf.Clamp(
            pos.z,
            corridor.position.z - halfLength,
            corridor.position.z + halfLength
        );

        transform.position = pos;
    }
}


}
