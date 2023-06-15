using UnityEngine;

public class BoidsManager : MonoBehaviour
{
    public GameObject boidPrefab;
    public int numBoids = 20;
    public GameObject[] allBoids;
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
        allBoids = new GameObject[numBoids];
        goalPos = goal.transform.position;

        for (int i = 0; i < numBoids; ++i)
        {

            Vector3 pos = transform.position + new Vector3(
                Random.Range(-spawnRangeLimits.x, spawnRangeLimits.x),
                Random.Range(-spawnRangeLimits.y, spawnRangeLimits.y),
                Random.Range(-spawnRangeLimits.z, spawnRangeLimits.z));

            allBoids[i] = Instantiate(boidPrefab, pos, Quaternion.identity);
            allBoids[i].GetComponent<BoidsAgent>().Initialize(this);
        }
    }
}