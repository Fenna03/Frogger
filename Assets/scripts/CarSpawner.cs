using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab; // The car prefab to spawn
    public float spawnInterval = 1f; // Time between spawning cars
    public float[] spawnYPositions; // Array of y-coordinates where cars will spawn
    public float spawnXLeft = -10f; // X-coordinate for spawning cars on the left
    public float spawnDelay = 0.5f; // Delay between spawning individual cars
    public float minSpeed = 2f; // Minimum speed for cars
    public float maxSpeed = 6f; // Maximum speed for cars

    private float[] laneSpeeds; // Array of speeds corresponding to each Y-coordinate

    private void Start()
    {
        // Initialize the laneSpeeds array
        laneSpeeds = new float[spawnYPositions.Length];

        // Randomly generate speeds for each lane
        for (int i = 0; i < laneSpeeds.Length; i++)
        {
            laneSpeeds[i] = Random.Range(minSpeed, maxSpeed);
        }

        StartCoroutine(SpawnCars());
    }

    private IEnumerator SpawnCars()
    {
        while (true)
        {
            // Spawn cars at different intervals
            for (int i = 0; i < spawnYPositions.Length; i++)
            {
                float yPos = spawnYPositions[i];
                Vector3 spawnPosition = new Vector3(spawnXLeft, yPos, 0f);

                GameObject newCar = Instantiate(carPrefab, spawnPosition, Quaternion.identity);

                // Set the speed of the car based on the Y-coordinate
                newCar.GetComponent<CarMovement>().SetSpeed(laneSpeeds[i]);

                yield return new WaitForSeconds(spawnDelay);
            }

            // Wait before starting the next wave of cars
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}