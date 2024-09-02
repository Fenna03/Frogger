using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 5f; // Speed at which the car moves
    public float leftLimit = -15f; // Left boundary of the screen
    public float rightLimit = 15f; // Right boundary of the screen

    private void Update()
    {
        // Move the car
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Destroy the car once it moves out of bounds
        if (transform.position.x > rightLimit)
        {
            DestroyCar();
        }
    }

    private void DestroyCar()
    {
        // When destroying the car, free up its Y position
        CarSpawner spawner = FindObjectOfType<CarSpawner>();
        if (spawner != null)
        {
            spawner.occupiedPositions.Remove(transform.position.y);
        }

        Destroy(gameObject);
    }
}