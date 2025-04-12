using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform car;  // Reference to the car object
    public Vector3 offset; // Offset from the car's position

    void LateUpdate()
    {
        // Update the camera position to match the car's position plus the offset
        transform.position = car.position + offset;

        // Make the camera look at the car
        transform.LookAt(car);
    }
}
