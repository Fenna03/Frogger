using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private float speed = 5f; // Speed at which the car moves
    public float rightLimit = 15f; // Right boundary of the screen

    private void Update()
    {
        // Move the car
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Check if the car has moved out of the screen and destroy it
        if (transform.position.x > rightLimit)
        {
            Destroy(gameObject);
        }
    }

    // Method to set the car's speed dynamically
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
