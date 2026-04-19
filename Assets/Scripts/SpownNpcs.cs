using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownNpcs : MonoBehaviour
{
    [Header("Prefab & Points")]
    public GameObject CarNpcPrefab;
    public Transform destinationPoint;

    [Header("Spawn Settings")]
    public bool spawnOnStart = true;
    public int maxSimultaneous = 10;
    public float spawnInterval = 3f;
    public float spawnRadius = 0f; // 0 => exactly at spawner position

    [Header("Movement (fallback)")]
    public float carMoveSpeed = 6f;
    public bool destroyOnArrival = true;
    public float destroyDelay = 1f;

    List<Transform> activeCars = new List<Transform>();
    bool spawning = false;

    void Start()
    {
        if (CarNpcPrefab == null)
        {
            Debug.LogError("[SpownNpcs] CarNpcPrefab is null. Assign a prefab in the inspector.");
            enabled = false;
            return;
        }

        if (destinationPoint == null)
        {
            Debug.LogError("[SpownNpcs] destinationPoint is null. Assign a Transform destination in the inspector.");
            enabled = false;
            return;
        }

        if (spawnOnStart)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        if (!spawning)
        {
            spawning = true;
            StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawning()
    {
        spawning = false;
        StopAllCoroutines();
    }

    IEnumerator SpawnLoop()
    {
        while (spawning)
        {
            // limit simultaneous
            activeCars.RemoveAll(c => c == null);
            if (activeCars.Count < maxSimultaneous)
            {
                SpawnOne();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOne()
    {
        Vector3 pos = transform.position;
        if (spawnRadius > 0f)
        {
            Vector2 circle = Random.insideUnitCircle * spawnRadius;
            pos += new Vector3(circle.x, 0f, circle.y);
        }

        GameObject go = Instantiate(CarNpcPrefab, pos, Quaternion.identity);
        activeCars.Add(go.transform);

        // Try NavMeshAgent if present
        var agent = go.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(destinationPoint.position);
            return;
        }

        // Fallback: rotate to face destination so visual orientation is correct
        go.transform.LookAt(destinationPoint.position);
    }

    void Update()
    {
        if (activeCars.Count == 0) return;

        for (int i = activeCars.Count - 1; i >= 0; i--)
        {
            var t = activeCars[i];
            if (t == null)
            {
                activeCars.RemoveAt(i);
                continue;
            }

            // If the spawned object has a NavMeshAgent, skip transform movement (agent moves it)
            var agent = t.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) continue;

            // Move transform towards destination (simple fallback movement)
            Vector3 target = destinationPoint.position;
            float step = carMoveSpeed * Time.deltaTime;
            t.position = Vector3.MoveTowards(t.position, target, step);

            // Optional: keep the object oriented towards movement
            Vector3 dir = target - t.position;
            if (dir.sqrMagnitude > 0.001f)
            {
                t.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            }

            // Arrival handling
            if (Vector3.Distance(t.position, target) <= 0.1f)
            {
                if (destroyOnArrival)
                {
                    Destroy(t.gameObject, destroyDelay);
                }
                activeCars.RemoveAt(i);
            }
        }
    }
}
