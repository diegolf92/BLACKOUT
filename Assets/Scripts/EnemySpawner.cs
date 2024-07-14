using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public float spawnRate = 2f;   // Rate of spawning enemies (in seconds)
    public float spawnYRange = 4f; // Range of y-axis positions for spawning enemies
    public Transform cameraPos;
    public float spawnXOffset = 1f; // Offset from the right edge of the screen to spawn enemies

    private float nextSpawnTime;   // Time to spawn the next enemy

    void Start()
    {
        // Initialize nextSpawnTime to the current time + spawnRate
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        // Check if it's time to spawn a new enemy
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            // Calculate the next spawn time
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnEnemy()
    {
        float camPos = cameraPos.position.x + spawnXOffset;
        // Calculate random y-axis position within the range
        float spawnY = Random.Range(-spawnYRange, spawnYRange);

        // Calculate spawn position
        Vector3 spawnPosition = new Vector3(camPos, spawnY, 0f);

        // Spawn the enemy
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
