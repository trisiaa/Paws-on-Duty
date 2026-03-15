using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;

    public Transform maya;
    public float DP = 2.5f;

    public Transform[] corridors;

    Animator anim;

    Transform currentCorridor;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        if (corridors.Length > 0)
            currentCorridor = corridors[0];
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, maya.position);

        float currentSpeed = speed;

        if (distance > DP)
        {
            currentSpeed = speed * 0.2f;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (move.magnitude > 0.05f)
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

        UpdateCurrentCorridor();
        BatasiCorridor();
    }

    void UpdateCurrentCorridor()
    {
        Vector3 pos = transform.position;

        foreach (Transform corridor in corridors)
        {
            float halfWidth = corridor.localScale.x / 2f;
            float halfLength = corridor.localScale.z / 2f;

            float minX = corridor.position.x - halfWidth;
            float maxX = corridor.position.x + halfWidth;

            float minZ = corridor.position.z - halfLength;
            float maxZ = corridor.position.z + halfLength;

            if (pos.x >= minX && pos.x <= maxX &&
                pos.z >= minZ && pos.z <= maxZ)
            {
                currentCorridor = corridor;
                return;
            }
        }
    }

    void BatasiCorridor()
    {
        if (currentCorridor == null) return;

        Vector3 pos = transform.position;

        float halfWidth = currentCorridor.localScale.x / 2f;
        float halfLength = currentCorridor.localScale.z / 2f;

        float minX = currentCorridor.position.x - halfWidth;
        float maxX = currentCorridor.position.x + halfWidth;

        float minZ = currentCorridor.position.z - halfLength;
        float maxZ = currentCorridor.position.z + halfLength;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}