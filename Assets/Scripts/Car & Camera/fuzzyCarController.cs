using UnityEngine;
using DotFuzzy;

public class FuzzyCarController : MonoBehaviour
{
    public FuzzyEngineController fuzzyEngineController;  // Reference to FuzzyEngineController
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float motorTorqueMultiplier = 20f;
    public float maxSteeringAngle = 30f;

    // Reference to the ParkingSpot component in the scene
    public ParkingSpot parkingSpot;

    private void Start()
    {
        // Ensure fuzzyEngineController and fuzzyEngine are assigned
        if (fuzzyEngineController == null)
        {
            Debug.LogError("FuzzyEngineController is not assigned!");
        }
        else if (fuzzyEngineController.fuzzyEngine == null)
        {
            Debug.LogError("FuzzyEngine is not assigned in FuzzyEngineController!");
        }

        if (parkingSpot == null)
        {
            Debug.LogError("ParkingSpot reference is not assigned in FuzzyCarController!");
        }
    }

    void FixedUpdate()
    {
        if (fuzzyEngineController == null || fuzzyEngineController.fuzzyEngine == null)
        {
            Debug.LogError("FuzzyEngineController or FuzzyEngine is null!");
            return;
        }

        if (parkingSpot == null)
        {
            Debug.LogError("ParkingSpot reference is missing!");
            return;
        }

        // Get the measured angle to the parking spot.
        float angleToParkingSpot = GetAngleToParkingSpot();
        // Get the measured distance from the car to the parking spot center.
        float distanceToParkingSpot = MeasureDistanceToParkingSpot();

        // Set the fuzzy logic inputs.
        fuzzyEngineController.fuzzyEngine.SetInput("AngleToParkingSpot", angleToParkingSpot);
        fuzzyEngineController.fuzzyEngine.SetInput("DistanceToObstacle", distanceToParkingSpot);

        // Evaluate the fuzzy logic system.
        fuzzyEngineController.fuzzyEngine.Evaluate();

        // Get crisp outputs for steering and speed.
        float fuzzySteering = (float)fuzzyEngineController.fuzzyEngine.GetOutput("Steering");
        float fuzzySpeed = (float)fuzzyEngineController.fuzzyEngine.GetOutput("Speed");
        Debug.Log(fuzzyEngineController.fuzzyEngine.GetOutput("Speed"));

        Debug.Log($"Angle: {angleToParkingSpot:F2}, Distance: {distanceToParkingSpot:F2}, Steering: {fuzzySteering:F2}, Speed: {fuzzySpeed:F2}");

        // Apply the controls to the car.
        ApplyCarControls(fuzzySpeed, fuzzySteering);
    }

    /// <summary>
    /// Computes the angle between the car's forward direction and the direction toward the parking spot.
    /// </summary>
    private float GetAngleToParkingSpot()
    {
        Vector3 carDirection = transform.forward;
        Vector3 toParkingSpot = (GameObject.Find("center").transform.position - transform.position).normalized;

        float angle = Vector3.SignedAngle(carDirection, toParkingSpot, Vector3.up);

        // Clamp to avoid unexpected values
        return Mathf.Clamp(angle, -180f, 180f);
    }


    /// <summary>
    /// Computes the distance from the car's current position to the parking spot's center.
    /// </summary>
    private float MeasureDistanceToParkingSpot()
    {
        if (parkingSpot == null || parkingSpot.parkingSpotCenter == null)
        {
            Debug.LogError("ParkingSpot or its parkingSpotCenter is not assigned!");
            return 0f;
        }

        float distance = Vector3.Distance(transform.position, parkingSpot.parkingSpotCenter.position);
        return distance;
    }

    /// <summary>
    /// Applies the fuzzy outputs to the car's motor torque and steering.
    /// </summary>
    private void ApplyCarControls(float speed, float steering)
    {
        float appliedTorque = speed * motorTorqueMultiplier;
        float appliedSteering = Mathf.Clamp(steering, -maxSteeringAngle, maxSteeringAngle);

        // Apply motor torque to rear wheels.
        rearLeftWheel.motorTorque = appliedTorque;
        rearRightWheel.motorTorque = appliedTorque;

        // Apply steering angle to front wheels.
        frontLeftWheel.steerAngle = appliedSteering;
        frontRightWheel.steerAngle = appliedSteering;
    }
}
