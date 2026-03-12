using UnityEngine;

public class CarSensor : MonoBehaviour
{
    public CarController car;

    int playerCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "NPC_Maya" || other.name == "Player_Max")
        {
            playerCount++;
            car.StopCar();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "NPC_Maya" || other.name == "Player_Max")
        {
            playerCount--;

            if (playerCount <= 0)
            {
                car.ResumeCar();
            }
        }
    }
}