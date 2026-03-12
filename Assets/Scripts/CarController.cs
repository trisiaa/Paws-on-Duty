using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 6f;

    bool stop = false;

    void Update()
    {
        if (stop) return;

        transform.position += Vector3.right * speed * Time.deltaTime;
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