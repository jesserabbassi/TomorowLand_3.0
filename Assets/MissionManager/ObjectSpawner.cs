using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;   // M points
    public GameObject prefab;         // object to spawn
    public int N = 3;                 // how many to pick

    void Start()
    {
        SpawnRandomPoints();
    }

    void SpawnRandomPoints()
    {
        if (N > spawnPoints.Length)
            N = spawnPoints.Length;

        List<Transform> available = new List<Transform>(spawnPoints);

        for (int i = 0; i < N; i++)
        {
            int index = Random.Range(0, available.Count);

            Transform chosen = available[index];

            Instantiate(prefab, chosen.position, chosen.rotation);

            available.RemoveAt(index); // ensures no duplicates
        }
    }
}