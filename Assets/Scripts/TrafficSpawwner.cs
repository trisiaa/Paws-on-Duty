using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    public GameObject[] cars;
    public Transform spawnPoint;
    public Transform pathParent;

    public float spawnDelay = 5f;

    Transform[] waypoints;

    void Start()
    {
        int count = pathParent.childCount;
        waypoints = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            waypoints[i] = pathParent.GetChild(i);
        }

        InvokeRepeating("SpawnCar", 2f, spawnDelay);
    }

    void SpawnCar()
    {
        // cek apakah ada mobil dekat spawn
        Collider[] hits = Physics.OverlapSphere(spawnPoint.position, 3f);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Car"))
            {
                return; // jangan spawn jika masih macet
            }
        }

        int rand = Random.Range(0, cars.Length);

        GameObject car = Instantiate(
            cars[rand],
            spawnPoint.position,
            spawnPoint.rotation
        );

        CarController controller = car.GetComponent<CarController>();
        controller.waypoints = waypoints;
    }
}