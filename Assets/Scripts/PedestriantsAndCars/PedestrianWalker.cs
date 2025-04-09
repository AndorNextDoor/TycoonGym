using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianWalker : MonoBehaviour
{
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        foreach(Transform child in PedestrianWayPointsAndSpawn.Instance.GetRandomWay())
        {
            waypoints.Add(child);
        }

        if(waypoints.Count <= 0)
        {
            Destroy(gameObject);
        }

        transform.position = waypoints[0].position;
        agent.SetDestination(waypoints[1].position);
        currentWaypointIndex = 1;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Count)
        {
            Destroy(gameObject);
            return;
        }

        agent.SetDestination(waypoints[currentWaypointIndex].position);

    }
}
