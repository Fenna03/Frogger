using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera follows (usually the player)
    public float smoothSpeed = 0.125f; // Smoothness of camera movement
    public Vector3 offset; // Offset from the target

    private void FixedUpdate()
    {
        if (target != null)
        {
            // Calculate desired position while keeping the z-axis fixed
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.z = transform.position.z; // Lock the z-axis

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void StopFollowing()
    {
        target = null; // Stop following the target
    }
}
