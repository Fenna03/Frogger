using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab; // The car prefab to spawn
    public float spawnInterval = 1f; // Time between spawning cars
    public float[] spawnYPositions; // Array of y-coordinates where cars will spawn
    public float spawnXLeft = -10f; // X-coordinate for spawning cars on the left
    public float spawnDelay = 0.5f; // Delay between spawning individual cars
    public int initialCarCount = 6; // Number of cars initially on the road

    public HashSet<float> occupiedPositions = new HashSet<float>();

    private void Start()
    {
        // Spawn initial cars
        for (int i = 0; i < initialCarCount; i++)
        {
            SpawnInitialCar();
        }

        // Start the coroutine for continuous car spawning
        StartCoroutine(SpawnCars());
    }

    private void SpawnInitialCar()
    {
        // Find a random position that is not occupied
        float randomY;
        do
        {
            randomY = spawnYPositions[Random.Range(0, spawnYPositions.Length)];
        } while (occupiedPositions.Contains(randomY));

        // Mark this position as occupied
        occupiedPositions.Add(randomY);

        // Spawn the car at the designated position
        Vector3 spawnPosition = new Vector3(spawnXLeft, randomY, 0f);
        Instantiate(carPrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator SpawnCars()
    {
        while (true)
        {
            // Spawn cars at different intervals, avoiding occupied positions
            for (int i = 0; i < spawnYPositions.Length; i++)
            {
                float randomY;
                do
                {
                    randomY = spawnYPositions[Random.Range(0, spawnYPositions.Length)];
                } while (occupiedPositions.Contains(randomY));

                // Mark this position as occupied
                occupiedPositions.Add(randomY);

                // Spawn the car at the designated position
                Vector3 spawnPosition = new Vector3(spawnXLeft, randomY, 0f);
                Instantiate(carPrefab, spawnPosition, Quaternion.identity);

                yield return new WaitForSeconds(spawnDelay);
            }

            // Wait before starting the next wave of cars
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}