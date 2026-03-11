using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = new Vector3(0,12,-10);

    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(45,0,0);
    }
}