using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; 
    public Transform player;
    public Transform spawnPoint;
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;
    public float spawnOffset = 10f; 
    public float obstacleSpeed = 5f; 
    void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            Vector3 spawnPosition = spawnPoint.position;
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

            Rigidbody rb = newObstacle.GetComponent<Rigidbody>();
            rb.velocity = -transform.right * obstacleSpeed;

            float randomSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }
}
