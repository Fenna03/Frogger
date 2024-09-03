using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab; // The car prefab to spawn
    public GameObject waterPrefab; // The water prefab to spawn
    public float spawnDelay = 0.5f; // Delay between spawning individual cars
    public float laneHeight = 1f; // Height of each lane
    public int minLanes = 3; // Minimum number of lanes to spawn cars on
    public int maxLanes = 7; // Maximum number of lanes to spawn cars on
    public LaneSpawner laneSpawner; // Reference to the LaneSpawner script

    public float waterLogSpacing = 5f; // Distance between water logs
    public float waterSpawnXLeft = -10f; // X-coordinate to start spawning water logs on the left
    public float waterSpawnXRight = 11f; // X-coordinate to start spawning water logs on the right

    // Dictionary to store speeds for different lanes
    private Dictionary<float, float> laneSpeeds = new Dictionary<float, float>();

    private void Start()
    {
        // Initialize car speeds for different lanes
        InitializeLaneSpeeds();

        StartCoroutine(SpawnEntities());
    }

    private void InitializeLaneSpeeds()
    {
        // Define different speeds for each lane
        for (float y = 1f; y <= 10f; y += laneHeight) // Adjust range and increment as needed
        {
            float speed = Random.Range(1f, 5f); // Random speed between 1 and 5 for example
            laneSpeeds[y] = speed;
        }
    }

    private IEnumerator SpawnEntities()
    {
        while (true)
        {
            // Determine available lanes for cars
            var availableCarLanes = GetAvailableLanesForCars();

            // Determine blue lanes for water
            var blueLanes = GetBlueLanes();

            // Spawn cars on random lanes
            foreach (float y in availableCarLanes)
            {
                Vector3 spawnPosition = new Vector3(waterSpawnXLeft, y, 0f);
                GameObject car = Instantiate(carPrefab, spawnPosition, Quaternion.identity);

                // Set the car's speed based on the lane's y-coordinate
                CarMovement carMovement = car.GetComponent<CarMovement>();
                if (carMovement != null)
                {
                    carMovement.SetSpeed(laneSpeeds[y]);
                }

                yield return new WaitForSeconds(spawnDelay);
            }

            // Spawn water logs on blue lanes, with alternating directions
            for (int i = 0; i < blueLanes.Count; i++)
            {
                float y = blueLanes[i];
                float xOffset = (i % 2 == 0) ? waterSpawnXLeft : waterSpawnXRight; // Start position based on direction

                // Determine the direction of movement based on index
                bool moveLeftToRight = i % 2 == 0;

                // Spawn water logs at the starting xOffset and spaced out
                while (moveLeftToRight ? xOffset < 0 : xOffset > -waterLogSpacing * 5) // Ensure logs move off-screen
                {
                    Vector3 spawnPosition = new Vector3(xOffset, y, 0f);
                    GameObject waterLog = Instantiate(waterPrefab, spawnPosition, Quaternion.identity);

                    // Set movement direction
                    WaterPrefab movement = waterLog.GetComponent<WaterPrefab>();
                    if (movement != null)
                    {
                        movement.SetDirection(moveLeftToRight);
                    }

                    // Increment xOffset by spacing
                    xOffset += moveLeftToRight ? waterLogSpacing : -waterLogSpacing;

                    yield return new WaitForSeconds(spawnDelay); // Optional delay between logs
                }
            }

            // Wait before starting the next round of spawns
            yield return new WaitForSeconds(spawnDelay * (availableCarLanes.Count + blueLanes.Count));
        }
    }

    private List<float> GetAvailableLanesForCars()
    {
        List<float> availableLanes = new List<float>();

        for (float y = 1f; y <= 10f; y += laneHeight) // Adjust max value as needed
        {
            if (!laneSpawner.IsLaneBlue(y)) // Exclude blue lanes for cars
            {
                availableLanes.Add(y);
            }
        }

        availableLanes = ShuffleList(availableLanes);

        int laneCount = Random.Range(minLanes, maxLanes + 1);
        if (availableLanes.Count > laneCount)
        {
            availableLanes = availableLanes.GetRange(0, laneCount);
        }

        return availableLanes;
    }

    private List<float> GetBlueLanes()
    {
        List<float> blueLanes = new List<float>();

        for (float y = 1f; y <= 10f; y += laneHeight) // Adjust max value as needed
        {
            if (laneSpawner.IsLaneBlue(y)) // Include only blue lanes
            {
                blueLanes.Add(y);
            }
        }

        return blueLanes;
    }

    private List<float> ShuffleList(List<float> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            float temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}

