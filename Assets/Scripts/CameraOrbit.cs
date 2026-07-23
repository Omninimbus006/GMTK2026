using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; 

    [Header("Orbit Controls")]
    public float speed = 20.0f; 
    public float diameter = 10.0f;
    public float height = 5.0f;

    private float currentAngle = 0.0f;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Calculate the radius from diameter
        float radius = diameter / 2f;

        // 2. Increment the rotation angle over time
        currentAngle += speed * Time.deltaTime;

        // 3. Convert polar coordinates to 3D positions (X and Z circle)
        float xOffset = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
        float zOffset = -Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;

        // 4. Set the final position relative to the target's position
        Vector3 newPosition = new Vector3(
            target.position.x + xOffset,
            target.position.y + height,
            target.position.z + zOffset
        );
        transform.position = newPosition;

        // 5. Force the lens to look directly at the center of the target
        transform.LookAt(target.position);
    }
}
