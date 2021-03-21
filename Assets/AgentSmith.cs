using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentSmith : MonoBehaviour
{
    private Waypoints[] waypoints;
    private NavMeshAgent agent;

    private Waypoints RandomWaypoint => waypoints[Random.Range(0, waypoints.Length)];
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        waypoints = FindObjectsOfType<Waypoints>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(RandomWaypoint.Position);  
        }
    }
}
