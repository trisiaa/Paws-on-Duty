using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 6f;
    public float rotationSpeed = 5f;

    public Transform[] waypoints;

    int index = 0;
    bool stop = false;

    void Start()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            Vector3 dir = waypoints[0].position - transform.position;
            dir.y = 0;

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(0,180,0);
        }
    }

    void Update()
    {
        if (stop) return;
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[index];

        // Gerak menuju waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime);

        // arah menuju waypoint
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction) * Quaternion.Euler(0,180,0);

            Vector3 rot = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime).eulerAngles;

            transform.rotation = Quaternion.Euler(0, rot.y, 0);
        }

        // cek waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            index++;

            if (index >= waypoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StopCar()
    {
        stop = true;
    }

    public void ResumeCar()
    {
        stop = false;
    }
}