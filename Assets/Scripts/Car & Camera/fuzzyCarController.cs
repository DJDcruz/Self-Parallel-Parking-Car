using UnityEngine;
using DotFuzzy;

public class FuzzyCarController : MonoBehaviour
{
    public FuzzyEngineController fuzzyEngineController;  // Reference to FuzzyEngineController
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float motorTorque = 1500f;
    public float maxSteeringAngle = 30f;

    private void Start()
    {
        // Ensure fuzzyEngineController and fuzzyEngine are assigned
        if (fuzzyEngineController == null)
        {
            Debug.LogError("FuzzyEngineController is not assigned!");
        }

        if (fuzzyEngineController.fuzzyEngine == null)
        {
            Debug.LogError("FuzzyEngine is not assigned!");
        }
    }

    void FixedUpdate()
    {

        if (fuzzyEngineController == null || fuzzyEngineController.fuzzyEngine == null)
        {
            Debug.LogError("FuzzyEngineController or FuzzyEngine is null!");
            return;
        }

<<<<<<< Updated upstream
        float angleToParkingSpot = GetAngleToParkingSpot();

        // Set the input values for fuzzy logic
        fuzzyEngineController.fuzzyEngine.SetInput("AngleToParkingSpot", angleToParkingSpot);
=======
        if (parkingSpot == null)
        {
            Debug.LogError("ParkingSpot reference is missing!");
            return;
        }

        /// <summary>
        /// Computes the angle between the car's forward direction and the direction toward the parking spot.
        /// </summary>
        Vector3 toTarget = parkingSpot.parkingSpotCenter.position - transform.position;

        float lateralOffset = Vector3.Dot(transform.right, toTarget);    // +ve = right of spot, -ve = left
        float verticalOffset = Vector3.Dot(transform.forward, toTarget); // +ve = ahead of spot, -ve = behind    


        // Set the fuzzy logic inputs.
        fuzzyEngineController.fuzzyEngine.SetInput("LateralOffset", lateralOffset);
        fuzzyEngineController.fuzzyEngine.SetInput("VerticalOffset", verticalOffset);
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

        // Evaluate the fuzzy logic system
        fuzzyEngineController.fuzzyEngine.Evaluate();

        // Get the crisp output values for car speed and steering angle
        float steeringAngle = (float)fuzzyEngineController.fuzzyEngine.GetOutput("SteeringAngle");
        

<<<<<<< Updated upstream
<<<<<<< Updated upstream
        Debug.Log($"Angle: {angleToParkingSpot}, Torque: {rearLeftWheel.motorTorque}, Steer Angle: {frontLeftWheel.steerAngle}");

        // Apply the controls to the car
        ApplyCarControls(-50f, steeringAngle);
=======
        Debug.Log($"Lateral Offset: {lateralOffset:F2}, Vertical Offset: {verticalOffset:F2}, Steering: {fuzzySteering:F2}, Speed: {fuzzySpeed:F2}");
>>>>>>> Stashed changes
=======
        Debug.Log($"Lateral Offset: {lateralOffset:F2}, Vertical Offset: {verticalOffset:F2}, Steering: {fuzzySteering:F2}, Speed: {fuzzySpeed:F2}");
>>>>>>> Stashed changes

    }

<<<<<<< Updated upstream
<<<<<<< Updated upstream

    private float GetAngleToParkingSpot()
    {

        // Perform a sphere cast


           
            Vector3 carDirection = transform.forward;

            // Calculate the angle between the car's direction and the parking spot
            Vector3 toParkingSpot = (GameObject.Find("center").transform.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(transform.forward, toParkingSpot, Vector3.up);
        return angle; // Return the angle to the parking spot
    }


=======
=======
>>>>>>> Stashed changes
    /// <summary>
    /// Applies the fuzzy outputs to the car's motor torque and steering.
    /// </summary>
>>>>>>> Stashed changes
    private void ApplyCarControls(float speed, float steering)
    {
        // Apply motor torque and steering angle based on fuzzy logic output

        rearLeftWheel.motorTorque = -200;
        rearRightWheel.motorTorque = -200;

        frontLeftWheel.steerAngle = steering;
        frontRightWheel.steerAngle = steering;
    }

}
