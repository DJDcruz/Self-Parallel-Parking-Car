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

    }
}
