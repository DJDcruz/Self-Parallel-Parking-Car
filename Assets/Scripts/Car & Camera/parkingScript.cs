using UnityEngine;

public class ParkingSpot : MonoBehaviour
{
    public Transform parkingSpotCenter; // Center point of the spot
    public float tolerance = 5f;      // Distance threshold for parking accuracy
    public float angleTolerance = 180f;  // Angle threshold for alignment

    // Reference to the car (optional, if managed elsewhere)
    private MeshRenderer meshRenderer; // MeshRenderer of the parking spot

    void Start()
    {
        // Get the MeshRenderer component of the parking spot
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("No MeshRenderer attached to the parking spot!");
        }
    }
    public Transform car;

     void FixedUpdate()
    {
        // Check parking status if a car reference is available
        if (car != null && meshRenderer != null)
        {
            bool isParked = IsCarParked(car);

            // Change color based on parking status
            if (isParked)
            {
                meshRenderer.material.color = Color.green; // Turn green when parked
                Time.timeScale = 0f;
            }
            else
            {
                meshRenderer.material.color = Color.red; // Turn red when not parked
            }

            Debug.Log($"Is car parked: {isParked}");
        }
    }

    public bool IsCarParked(Transform car)
    {
        // Check position
        float distance = Vector3.Distance(car.position, parkingSpotCenter.position);
        bool isPositionCorrect = distance <= tolerance;

        // Check alignment
        Vector3 forward = car.forward;
        Vector3 spotForward = parkingSpotCenter.forward;
        float angle = Vector3.Angle(forward, spotForward);
        bool isAngleCorrect = angle <= angleTolerance;

        return isPositionCorrect && isAngleCorrect;
    }

    private void OnDrawGizmos()
    {
        if (parkingSpotCenter == null)
            return;

        // Draw position tolerance zone
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(parkingSpotCenter.position, new Vector3(tolerance * 2, 0.1f, tolerance * 2));

        // Draw alignment indicator (optional)
        Gizmos.color = Color.blue;
        Vector3 forward = parkingSpotCenter.forward * 2;
        Gizmos.DrawLine(parkingSpotCenter.position, parkingSpotCenter.position + forward);
    }
}
