using UnityEngine;
using DotFuzzy;

public class FuzzyEngineController : MonoBehaviour
{
    public FuzzyEngine fuzzyEngine; // Expose this for other scripts like FuzzyCarController to reference

    void Start()
    {
        fuzzyEngine = new FuzzyEngine();
        if (fuzzyEngine == null)
        {
            fuzzyEngine = new FuzzyEngine();
            // Make sure fuzzyEngine is instantiated
        }

<<<<<<< Updated upstream
        LinguisticVariable angleToParkingSpot = new LinguisticVariable("AngleToParkingSpot");
        angleToParkingSpot.MembershipFunctionCollection.Add(new MembershipFunction("Straight", -5, 0, 0, 5));
        angleToParkingSpot.MembershipFunctionCollection.Add(new MembershipFunction("SlightLeft", -20, -15, -10, -5));
        angleToParkingSpot.MembershipFunctionCollection.Add(new MembershipFunction("SharpLeft", -180, -120, -60, -40));
        angleToParkingSpot.MembershipFunctionCollection.Add(new MembershipFunction("SlightRight", 5, 10, 15, 20));
        angleToParkingSpot.MembershipFunctionCollection.Add(new MembershipFunction("SharpRight", 40, 60, 120, 180));

        LinguisticVariable steeringAngle = new LinguisticVariable("SteeringAngle");
        steeringAngle.MembershipFunctionCollection.Add(new MembershipFunction("SharpLeft", -100, -60, -30, -20));
        steeringAngle.MembershipFunctionCollection.Add(new MembershipFunction("Left", -15, -10, -5, 0));
        steeringAngle.MembershipFunctionCollection.Add(new MembershipFunction("Straight", -5, 0, 0, 5));
        steeringAngle.MembershipFunctionCollection.Add(new MembershipFunction("Right", 0, 5, 10, 15));
        steeringAngle.MembershipFunctionCollection.Add(new MembershipFunction("SharpRight", 20, 30, 60, 100));

        // Add Linguistic Variables to the Fuzzy Engine
        fuzzyEngine.LinguisticVariableCollection.Add(angleToParkingSpot);
        fuzzyEngine.LinguisticVariableCollection.Add(steeringAngle);
        fuzzyEngine.consequent = "steeringAngle";


        // Define Fuzzy Rules for Reversing into Parking Spot
        FuzzyRule rule1 = new FuzzyRule("IF (AngleToParkingSpot IS SharpLeft) THEN SteeringAngle IS SharpLeft");
        FuzzyRule rule2 = new FuzzyRule("IF (AngleToParkingSpot IS SlightLeft) THEN SteeringAngle IS Left");
        FuzzyRule rule3 = new FuzzyRule("IF (AngleToParkingSpot IS Straight) THEN SteeringAngle IS Straight");
        FuzzyRule rule4 = new FuzzyRule("IF (AngleToParkingSpot IS SlightRight) THEN SteeringAngle IS Right");
        FuzzyRule rule5 = new FuzzyRule("IF (AngleToParkingSpot IS SharpRight) THEN SteeringAngle IS SharpRight");


        // Add Rules to the Fuzzy Engine
        fuzzyEngine.FuzzyRuleCollection.Add(rule1);
        fuzzyEngine.FuzzyRuleCollection.Add(rule2);
        fuzzyEngine.FuzzyRuleCollection.Add(rule3);
        fuzzyEngine.FuzzyRuleCollection.Add(rule4);
        fuzzyEngine.FuzzyRuleCollection.Add(rule5);
=======
        // -----------------------------
        // Input: LateralOffset
        // -----------------------------
        LinguisticVariable lateralVar = new LinguisticVariable("LateralOffset");
        lateralVar.MembershipFunctionCollection.Add(new MembershipFunction("FarLeft", -2.0, -1.5, -1.2, -0.8));
        lateralVar.MembershipFunctionCollection.Add(new MembershipFunction("Left", -1.0, -0.6, -0.4, -0.1));
        lateralVar.MembershipFunctionCollection.Add(new MembershipFunction("Center", -0.2, 0.0, 0.0, 0.2));
        lateralVar.MembershipFunctionCollection.Add(new MembershipFunction("Right", 0.1, 0.4, 0.6, 1.0));
        lateralVar.MembershipFunctionCollection.Add(new MembershipFunction("FarRight", 0.8, 1.2, 1.5, 2.0));

        fuzzyEngine.InputVariableCollection.Add(lateralVar);

        // -----------------------------
        // Input: VerticalOffset
        // -----------------------------
        LinguisticVariable verticalVar = new LinguisticVariable("VerticalOffset");
        verticalVar.MembershipFunctionCollection.Add(new MembershipFunction("Behind", -3.0f, -2.5f, -1.5f, -1.0f));

        // Define "Near" to cover the region where the car is almost aligned (e.g., -1.0 to +1.0).
        verticalVar.MembershipFunctionCollection.Add(new MembershipFunction("Near", -1.0f, -0.5f, 0.5f, 1.0f));

        // Define "Forward" so that it only starts when the vertical offset is clearly positive (e.g., above 0.5).
        verticalVar.MembershipFunctionCollection.Add(new MembershipFunction("Forward", 0.5f, 1.0f, 3.5f, 4.0f));


        fuzzyEngine.InputVariableCollection.Add(verticalVar);


                // -----------------------------
        // Output: Speed (Reverse)
        // -----------------------------
        LinguisticVariable speedVar = new LinguisticVariable("Speed");
        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("ForwardSlow", 1, 2, 3, 4));
        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("ForwardModerate", 3, 5, 6, 8));
        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("ForwardFast", 7, 9, 10, 12));

        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("ReverseSlow", -5, -5, -3, -1));
        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("ReverseModerate", -8, -6, -5, -3));
        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("ReverseFast", -12, -10, -8, -6));
        fuzzyEngine.OutputVariableCollection.Add(speedVar);


        // -----------------------------
        // Output: Steering
        // -----------------------------
        LinguisticVariable steeringVar = new LinguisticVariable("Steering");
        steeringVar.MembershipFunctionCollection.Add(new MembershipFunction("SharpLeft", -60, -45, -30, -20));
        steeringVar.MembershipFunctionCollection.Add(new MembershipFunction("Left", -30, -20, -10, -5));
        steeringVar.MembershipFunctionCollection.Add(new MembershipFunction("Straight", -2, 0, 0, 2));
        steeringVar.MembershipFunctionCollection.Add(new MembershipFunction("Right", 5, 10, 20, 30));
        steeringVar.MembershipFunctionCollection.Add(new MembershipFunction("SharpRight", 20, 30, 45, 60));
        fuzzyEngine.OutputVariableCollection.Add(steeringVar);

        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS Center AND VerticalOffset IS Forward THEN Speed IS ForwardSlow AND Steering IS Straight"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS Right AND VerticalOffset IS Forward THEN Speed IS ForwardModerate AND Steering IS Right"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS Left AND VerticalOffset IS Forward THEN Speed IS ForwardModerate AND Steering IS Left"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS FarRight AND VerticalOffset IS Forward THEN Speed IS ForwardSlow AND Steering IS SharpRight"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS FarLeft AND VerticalOffset IS Forward THEN Speed IS ForwardSlow AND Steering IS SharpLeft"));

        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS FarLeft AND VerticalOffset IS Behind THEN Speed IS ReverseSlow AND Steering IS SharpLeft"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS Left AND VerticalOffset IS Behind THEN Speed IS ReverseModerate AND Steering IS Left"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS Right AND VerticalOffset IS Behind THEN Speed IS ReverseModerate AND Steering IS Right"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS FarRight AND VerticalOffset IS Behind THEN Speed IS ReverseSlow AND Steering IS SharpRight"));

        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS Center AND VerticalOffset IS Near THEN Speed IS ForwardSlow AND Steering IS Straight"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS Center AND VerticalOffset IS Forward THEN Speed IS ReverseSlow AND Steering IS SharpRight"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF LateralOffset IS Center AND VerticalOffset IS Behind THEN Speed IS ReverseSlow AND Steering IS SharpRight"));
>>>>>>> Stashed changes

    }
}
