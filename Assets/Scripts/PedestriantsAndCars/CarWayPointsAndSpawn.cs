using System.Collections;
using UnityEngine;

public class CarWayPointsAndSpawn : MonoBehaviour
{
    public static CarWayPointsAndSpawn Instance;
    [SerializeField] private Transform[] waypointsParent;

    [SerializeField] private GameObject[] cars;
    [SerializeField] private int minSpawnTimer;
    [SerializeField] private int maxSpawnTimer;

    [SerializeField] private Transform farAwaySpawnPoint;

    private void Awake()
    {
        Instance = this;
        StartCoroutine(SpawnRandomPedestrian());
    }

    IEnumerator SpawnRandomPedestrian()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTimer, maxSpawnTimer));
            Spawn();
        }
    }

    private void Spawn()
    {
        Instantiate(cars[Random.Range(0, cars.Length)], farAwaySpawnPoint.position, Quaternion.identity);
    }

    public Transform GetRandomWay()
    {
        return waypointsParent[Random.Range(0, waypointsParent.Length)];
    }
}
