using UnityEngine;
using System.Collections.Generic;

public class LaneSpawner : MonoBehaviour
{
    public GameObject[] lanePrefabs; // Array of lane prefabs to spawn (black and gray)
    public int numberOfLanes = 10; // Number of lanes to keep active
    public float laneHeight = 1f; // Height of each lane
    public Transform player; // Reference to the player object
    public float spawnOffset = 10f; // How far ahead to spawn new lanes

    private Queue<GameObject> activeLanes; // Queue to store active lanes
    private float nextSpawnPositionY;
    private int laneIndex = 0; // To keep track of the color order

    private void Start()
    {
        activeLanes = new Queue<GameObject>();

        // Initially spawn lanes
        for (int i = 0; i < numberOfLanes; i++)
        {
            SpawnLane(i * laneHeight);
        }

        nextSpawnPositionY = numberOfLanes * laneHeight;
    }

    private void Update()
    {
        // Check if we need to spawn a new lane
        if (player.position.y + spawnOffset > nextSpawnPositionY - (numberOfLanes * laneHeight))
        {
            RecycleLane();
        }
    }

    private void SpawnLane(float yPos)
    {
        // Choose the lane prefab in an alternating pattern
        GameObject lanePrefab = lanePrefabs[laneIndex % lanePrefabs.Length];

        // Instantiate the lane
        GameObject newLane = Instantiate(lanePrefab, new Vector3(0f, yPos, 0f), Quaternion.identity);

        // Add the lane to the queue of active lanes
        activeLanes.Enqueue(newLane);

        // Update the laneIndex to alternate between prefabs
        laneIndex++;
    }

    private void RecycleLane()
    {
        // Get the oldest lane
        GameObject oldLane = activeLanes.Dequeue();

        // Move the lane to the new spawn position
        oldLane.transform.position = new Vector3(0f, nextSpawnPositionY, 0f);

        // Re-add the lane to the queue
        activeLanes.Enqueue(oldLane);

        // Update the next spawn position
        nextSpawnPositionY += laneHeight;

        // Update the laneIndex to ensure alternating colors
        laneIndex++;
    }
}
