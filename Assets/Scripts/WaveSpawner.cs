using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int totalSpawnEnemies;
    public int numberOfRandomSpawnPoint;
    public float delayStart;
    public float spawnInterval;
    public int numberOfPowerUp;
}

public class WaveSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;          // Enemy spawn points (Point01..Point06)
    public Transform[] powerUpSpawnPoints;   // Power-up spawn points (center of map)
    public GameObject enemyPrefab;
    public GameObject[] powerUpPrefabs;      // Both power-up types

    public Wave[] waves;

    void Start()
    {
        if (waves == null || waves.Length == 0)
        {
            waves = new Wave[]
            {
                new Wave { totalSpawnEnemies = 4,  numberOfRandomSpawnPoint = 1, delayStart = 2f, spawnInterval = 2.0f, numberOfPowerUp = 0 },
                new Wave { totalSpawnEnemies = 6,  numberOfRandomSpawnPoint = 2, delayStart = 2f, spawnInterval = 2.0f, numberOfPowerUp = 1 },
                new Wave { totalSpawnEnemies = 8,  numberOfRandomSpawnPoint = 4, delayStart = 2f, spawnInterval = 2.0f, numberOfPowerUp = 1 },
                new Wave { totalSpawnEnemies = 10, numberOfRandomSpawnPoint = 6, delayStart = 5f, spawnInterval = 0.5f, numberOfPowerUp = 2 },
            };
        }

        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        for (int w = 0; w < waves.Length; w++)
        {
            Wave wave = waves[w];
            Debug.Log("=== Wave " + (w + 1) + " Start ===");

            // 1) Pick random spawn points for enemies this wave
            List<Transform> selectedSpawnPoints = GetRandomSpawnPoints(wave.numberOfRandomSpawnPoint);

            // 2) Spawn power-ups immediately at power-up spawn points
            for (int p = 0; p < wave.numberOfPowerUp; p++)
            {
                Transform randomPoint = powerUpSpawnPoints[Random.Range(0, powerUpSpawnPoints.Length)];
                GameObject randomPowerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
                Instantiate(randomPowerUp, randomPoint.position, Quaternion.identity);
            }

            // 3) Wait delayStart before spawning enemies
            yield return new WaitForSeconds(wave.delayStart);

            // 4) Spawn enemies using only the selected spawn points
            for (int e = 0; e < wave.totalSpawnEnemies; e++)
            {
                Transform spawnPoint = selectedSpawnPoints[Random.Range(0, selectedSpawnPoints.Count)];
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                if (e < wave.totalSpawnEnemies - 1)
                {
                    yield return new WaitForSeconds(wave.spawnInterval);
                }
            }

            Debug.Log("=== Wave " + (w + 1) + " End ===");

            yield return new WaitForSeconds(2f);
        }

        Debug.Log("=== All Waves Complete ===");
    }

    List<Transform> GetRandomSpawnPoints(int count)
    {
        List<Transform> available = new List<Transform>(spawnPoints);
        List<Transform> selected = new List<Transform>();

        count = Mathf.Min(count, available.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, available.Count);
            selected.Add(available[index]);
            available.RemoveAt(index);
        }

        return selected;
    }
}