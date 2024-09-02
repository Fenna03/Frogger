using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    public float moveDistance = 1f; // Distance of each move (one block)
    public float moveSpeed = 5f; // Speed at which the frog moves to the new position

    private Vector3 targetPosition; // Target position to move to

    private void Start()
    {
        targetPosition = transform.position; // Start with the current position as the target
    }

    private void Update()
    {
        // Check for input and set target position accordingly
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetTargetPosition(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetTargetPosition(Vector3.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetTargetPosition(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetTargetPosition(Vector3.right);
        }

        // Move the frog towards the target position
        MoveTowardsTarget();
    }

    private void SetTargetPosition(Vector3 direction)
    {
        // Calculate the new target position based on moveDistance
        targetPosition = transform.position + direction * moveDistance;
    }

    private void MoveTowardsTarget()
    {
        // Move towards the target position
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

}
