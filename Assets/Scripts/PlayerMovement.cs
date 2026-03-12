using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;

    public Transform maya;
    public float DP = 2.5f;

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
    }
}