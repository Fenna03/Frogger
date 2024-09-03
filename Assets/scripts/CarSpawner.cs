using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab; // The car prefab to spawn
    public float spawnDelay = 0.5f; // Delay between spawning individual cars
    public float spawnXLeft = -10f; // X-coordinate for spawning cars
    public int minY = 1; // Minimum y-coordinate for spawning cars
    public int maxY = 10; // Maximum y-coordinate for spawning cars
    public int maxConsecutiveLanes = 4; // Maximum number of consecutive lanes with cars
    public float laneHeight = 1f; // Height of each lane
    public float minSpeed = 2f; // Minimum car speed
    public float maxSpeed = 10f; // Maximum car speed

    private Queue<int> recentSpawnedLanes; // Queue to keep track of recent y-coordinates
    private Dictionary<int, float> laneSpeeds; // Dictionary to store speeds for each lane

    private void Start()
    {
        recentSpawnedLanes = new Queue<int>(maxConsecutiveLanes);
        laneSpeeds = new Dictionary<int, float>();
        StartCoroutine(SpawnCars());
    }

    private IEnumerator SpawnCars()
    {
        // Randomize the y-coordinates for the current set of lanes and assign speeds
        List<int> randomizedYPositions = RandomizeYPositions();

        while (true)
        {
            foreach (int y in randomizedYPositions)
            {
                if (!laneSpeeds.ContainsKey(y))
                {
                    laneSpeeds[y] = Random.Range(minSpeed, maxSpeed); // Assign a random speed to the lane
                }

                // Spawn a car at the given y-coordinate
                SpawnCarAtY(y, laneSpeeds[y]);
                yield return new WaitForSeconds(spawnDelay);
            }

            // Wait before starting the next round of car spawns
            yield return new WaitForSeconds(spawnDelay * randomizedYPositions.Count);

            // Randomize y-coordinates again for the next round
            randomizedYPositions = RandomizeYPositions();
        }
    }

    private List<int> RandomizeYPositions()
    {
        List<int> result = new List<int>();

        int consecutiveCount = 0;
        int lastY = -1;

        // Randomly generate y-coordinates and ensure constraints
        while (result.Count < 10) // Adjust the number as needed
        {
            int y = Random.Range(minY, maxY + 1); // Generate a random integer within the specified range

            if (y == lastY)
            {
                consecutiveCount++;
            }
            else
            {
                consecutiveCount = 1;
                lastY = y;
            }

            if (consecutiveCount <= maxConsecutiveLanes)
            {
                result.Add(y);
            }
        }

        return result;
    }

    private void SpawnCarAtY(int y, float speed)
    {
        Vector3 spawnPosition = new Vector3(spawnXLeft, y, 0f);
        GameObject car = Instantiate(carPrefab, spawnPosition, Quaternion.identity);
        CarMovement carMovement = car.GetComponent<CarMovement>();
        if (carMovement != null)
        {
            carMovement.SetSpeed(speed); // Set the car's speed based on the lane
        }
    }
}
