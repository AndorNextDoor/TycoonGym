using System.Collections;
using UnityEngine;

public class PedestrianWayPointsAndSpawn : MonoBehaviour
{
    public static PedestrianWayPointsAndSpawn Instance;
    [SerializeField] private Transform[] waypointsParent;


    [SerializeField] private GameObject[] pedestriants;
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
        Instantiate(pedestriants[Random.Range(0, pedestriants.Length)], farAwaySpawnPoint.position, Quaternion.identity);
    }

    public Transform GetRandomWay()
    {
        return waypointsParent[Random.Range(0, waypointsParent.Length)];
    }
}
