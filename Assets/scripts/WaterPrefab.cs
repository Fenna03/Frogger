using UnityEngine;

public class WaterPrefab : MonoBehaviour
{
    public float speed = 2f; // Speed of the water log

    private bool moveLeftToRight = true;

    private void Update()
    {
        // Move water log based on direction
        if (moveLeftToRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

    public void SetDirection(bool moveLeftToRight)
    {
        this.moveLeftToRight = moveLeftToRight;
    }
}

