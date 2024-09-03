using UnityEngine;
using System.Collections.Generic;

public class LaneSpawner : MonoBehaviour
{
    public GameObject[] lanePrefabs; // Array of lane prefabs to spawn (black, gray, and blue)
    public int numberOfLanes = 10; // Number of lanes to keep active
    public float laneHeight = 1f; // Height of each lane
    public Transform player; // Reference to the player object
    public float spawnOffset = 10f; // How far ahead to spawn new lanes

    private Queue<GameObject> activeLanes; // Queue to store active lanes
    private float nextSpawnPositionY;
    private int laneIndex = 0; // To keep track of the color order
    private bool isInBlueGroup = false; // Flag to track if we're currently in a blue group
    private int blueGroupCount = 0; // Counter for blue group lanes

    // Dictionary to track lane positions and their colors
    private Dictionary<float, Color> laneColors = new Dictionary<float, Color>();

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
        GameObject lanePrefab;
        Color laneColor;

        if (isInBlueGroup)
        {
            lanePrefab = lanePrefabs[2]; // Blue lane prefab
            laneColor = new Color(0.407f, 0.4f, 1f); // RGB values for #6866FF
            blueGroupCount++;
            if (blueGroupCount >= Random.Range(3, 8)) // Randomize the end of the blue group between 3 and 7 lanes
            {
                isInBlueGroup = false;
                blueGroupCount = 0;
            }
        }
        else
        {
            lanePrefab = lanePrefabs[laneIndex % (lanePrefabs.Length - 1)]; // Skip blue lane prefab for regular lanes
            laneColor = lanePrefab.GetComponent<SpriteRenderer>().color;

            // Start a blue group with a certain probability
            if (Random.Range(0, 10) < 2) // Adjust probability as needed
            {
                isInBlueGroup = true;
                blueGroupCount = 0;
                lanePrefab = lanePrefabs[2]; // Blue lane prefab
                laneColor = new Color(0.407f, 0.4f, 1f); // RGB values for #6866FF
            }
        }

        // Instantiate the lane
        GameObject newLane = Instantiate(lanePrefab, new Vector3(0f, yPos, 0f), Quaternion.identity);

        // Add the lane to the queue of active lanes
        activeLanes.Enqueue(newLane);

        // Track the lane color
        laneColors[yPos] = laneColor;

        // Update the laneIndex to alternate between prefabs
        laneIndex++;
    }


    private void RecycleLane()
    {
        // Get the oldest lane
        GameObject oldLane = activeLanes.Dequeue();

        // Move the lane to the new spawn position
        oldLane.transform.position = new Vector3(0f, nextSpawnPositionY, 0f);

        GameObject newLanePrefab;
        Color newLaneColor;

        if (isInBlueGroup)
        {
            newLanePrefab = lanePrefabs[2]; // Blue lane prefab
            newLaneColor = new Color(0.407f, 0.4f, 1f); // RGB values for #6866FF
            blueGroupCount++;
            if (blueGroupCount >= Random.Range(3, 8)) // Randomize the end of the blue group between 3 and 7 lanes
            {
                isInBlueGroup = false;
                blueGroupCount = 0;
            }
        }
        else
        {
            newLanePrefab = lanePrefabs[laneIndex % (lanePrefabs.Length - 1)]; // Skip blue lane prefab for regular lanes
            newLaneColor = newLanePrefab.GetComponent<SpriteRenderer>().color;

            // Start a blue group with a certain probability
            if (Random.Range(0, 10) < 2) // Adjust probability as needed
            {
                isInBlueGroup = true;
                blueGroupCount = 0;
                newLanePrefab = lanePrefabs[2]; // Blue lane prefab
                newLaneColor = new Color(0.407f, 0.4f, 1f); // RGB values for #6866FF
            }
        }

        // Update the prefab and instantiate the lane
        oldLane.GetComponent<SpriteRenderer>().sprite = newLanePrefab.GetComponent<SpriteRenderer>().sprite;
        oldLane.transform.position = new Vector3(0f, nextSpawnPositionY, 0f);

        // Track the lane color
        laneColors[nextSpawnPositionY] = newLaneColor;

        // Re-add the lane to the queue
        activeLanes.Enqueue(oldLane);

        // Update the next spawn position
        nextSpawnPositionY += laneHeight;

        // Update the laneIndex to alternate between prefabs
        laneIndex++;
    }


    public bool IsLaneBlue(float y)
    {
        Color blueColor = new Color(0.407f, 0.4f, 1f); // RGB values for #6866FF
        return laneColors.TryGetValue(y, out Color color) && color == blueColor;
    }

}