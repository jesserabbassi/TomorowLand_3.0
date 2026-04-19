using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    public float roamRadius = 40f;
    public float decisionDelay = 3f; // how often to pick new destination

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Start moving immediately
        SetRandomDestination();

        // Keep changing destination
        InvokeRepeating(nameof(SetRandomDestination), decisionDelay, decisionDelay);
    }

    void SetRandomDestination()
    {
        Vector3 randomPoint = GetRandomPoint(transform.position, roamRadius);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere * radius;
        randomDir += center;
        randomDir.y = center.y;
        return randomDir;
    }
}