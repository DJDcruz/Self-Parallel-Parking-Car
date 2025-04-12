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

        float angleToParkingSpot = GetAngleToParkingSpot();

        // Set the input values for fuzzy logic
        fuzzyEngineController.fuzzyEngine.SetInput("AngleToParkingSpot", angleToParkingSpot);

        // Evaluate the fuzzy logic system
        fuzzyEngineController.fuzzyEngine.Evaluate();

        // Get the crisp output values for car speed and steering angle
        float steeringAngle = (float)fuzzyEngineController.fuzzyEngine.GetOutput("SteeringAngle");
        

        Debug.Log($"Angle: {angleToParkingSpot}, Torque: {rearLeftWheel.motorTorque}, Steer Angle: {frontLeftWheel.steerAngle}");

        // Apply the controls to the car
        ApplyCarControls(-50f, steeringAngle);

    }


    private float GetAngleToParkingSpot()
    {

        // Perform a sphere cast


           
            Vector3 carDirection = transform.forward;

            // Calculate the angle between the car's direction and the parking spot
            Vector3 toParkingSpot = (GameObject.Find("center").transform.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(transform.forward, toParkingSpot, Vector3.up);
        return angle; // Return the angle to the parking spot
    }


    private void ApplyCarControls(float speed, float steering)
    {
        // Apply motor torque and steering angle based on fuzzy logic output

        rearLeftWheel.motorTorque = -200;
        rearRightWheel.motorTorque = -200;

        frontLeftWheel.steerAngle = steering;
        frontRightWheel.steerAngle = steering;
    }

}
