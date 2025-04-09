using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;
    public float speed = 10f;

    private void Start()
    {
        foreach (Transform child in CarWayPointsAndSpawn.Instance.GetRandomWay())
        {
            waypoints.Add(child);
        }

        if (waypoints.Count <= 0)
        {
            Destroy(gameObject);
        }

        transform.position = waypoints[0].position;
        currentWaypointIndex = 1;
    }

    void Update()
    {
        if (waypoints.Count == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        transform.LookAt(target);

        if (direction.magnitude < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }

        if (currentWaypointIndex >= waypoints.Count)
        {
            Destroy(gameObject);
        }
    }
}
