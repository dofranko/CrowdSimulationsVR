using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{
    public GameObject boidPrefab;
    public int numBoids = 20;
    public static List<GameObject> allBoids = new();
    public Vector3 spawnRangeLimits = new(5.0f, 5.0f, 5.0f);
    public GameObject goal;
    public Vector3 goalPos = Vector3.zero;

    [Header("Boid Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 10.0f)] public float avoideDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;
    [Range(1.0f, 30.0f)] public float goalRadious;
    void Start()
    {
        Random.InitState(2137);
        // allBoids = new GameObject[numBoids];
        goalPos = goal.transform.position;

        for (int i = 0; i < numBoids; ++i)
        {

            Vector3 pos = transform.position + new Vector3(
                Random.Range(-spawnRangeLimits.x, spawnRangeLimits.x),
                Random.Range(-spawnRangeLimits.y, spawnRangeLimits.y),
                Random.Range(-spawnRangeLimits.z, spawnRangeLimits.z));

            var boid = Instantiate(boidPrefab, pos, Quaternion.identity);
            boid.GetComponent<BoidsAgent>().Initialize(this);
            allBoids.Add(boid);
        }
    }
}