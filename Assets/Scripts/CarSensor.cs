using UnityEngine;

public class CarSensor : MonoBehaviour
{
    public CarController car;

    int stopCount = 0;

    bool IsValidObstacle(Collider other)
    {
        // NPC selalu dihitung
        if (other.name == "NPC_Maya" || other.name == "Player_Max")
        {
            return true;
        }

        if (other.CompareTag("Car"))
        {
            CarController otherCar = other.GetComponentInParent<CarController>();

            if (otherCar != null)
            {
                float facing = Vector3.Dot(car.transform.forward, otherCar.transform.forward);

                // jika mobil berhadapan (arah berlawanan), abaikan
                if (facing < -0.5f)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsValidObstacle(other))
        {
            stopCount++;
            car.StopCar();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsValidObstacle(other))
        {
            stopCount--;

            if (stopCount <= 0)
            {
                stopCount = 0;
                car.ResumeCar();
            }
        }
    }
}