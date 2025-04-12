using UnityEngine;
using DotFuzzy;

public class FuzzyEngineController : MonoBehaviour
{
    public FuzzyEngine fuzzyEngine;

    void Awake()
    {
        fuzzyEngine = new FuzzyEngine();

        // --------------------------------------------------
        // Configure Input Variable: AngleToParkingSpot
        // --------------------------------------------------
        LinguisticVariable angleVar = new LinguisticVariable("AngleToParkingSpot");
        // Membership functions for full 180° left to right
        angleVar.MembershipFunctionCollection.Add(new MembershipFunction("VeryNegative", -180, -180, -135, -90));
        angleVar.MembershipFunctionCollection.Add(new MembershipFunction("NegativeLarge", -135, -90, -90, -45));
        angleVar.MembershipFunctionCollection.Add(new MembershipFunction("NegativeSmall", -60, -30, -15, 0));
        angleVar.MembershipFunctionCollection.Add(new MembershipFunction("Zero", -5, 0, 0, 5));
        angleVar.MembershipFunctionCollection.Add(new MembershipFunction("PositiveSmall", 0, 15, 30, 60));
        angleVar.MembershipFunctionCollection.Add(new MembershipFunction("PositiveLarge", 45, 90, 90, 135));
        angleVar.MembershipFunctionCollection.Add(new MembershipFunction("VeryPositive", 90, 135, 180, 180));
        fuzzyEngine.InputVariableCollection.Add(angleVar);

        // -----------------------------
        // Input: DistanceToObstacle
        // -----------------------------
        LinguisticVariable distanceVar = new LinguisticVariable("DistanceToObstacle");
        distanceVar.MembershipFunctionCollection.Add(new MembershipFunction("Near", 0, 0, 2, 5));
        distanceVar.MembershipFunctionCollection.Add(new MembershipFunction("Medium", 3, 6, 8, 11));
        distanceVar.MembershipFunctionCollection.Add(new MembershipFunction("Far", 10, 15, 20, 25));
        fuzzyEngine.InputVariableCollection.Add(distanceVar);


                // -----------------------------
        // Output: Speed (Reverse)
        // -----------------------------
        LinguisticVariable speedVar = new LinguisticVariable("Speed");
        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("ForwardSlow", 1, 2, 3, 4));
        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("ForwardModerate", 3, 5, 6, 8));
        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("ForwardFast", 7, 9, 10, 12));

        speedVar.MembershipFunctionCollection.Add(new MembershipFunction("Stop", 0, 0, 0, 0));

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


        // -----------------------------
        // Fuzzy Rules for Reverse Motion
        // -----------------------------
        // Steering and speed rules based on expanded angle range
        // FORWARD motion rules (assume car is better aligned and can go forward)
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS Zero AND DistanceToObstacle IS Far THEN Speed IS ForwardFast AND Steering IS Straight"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS PositiveSmall AND DistanceToObstacle IS Far THEN Speed IS ForwardModerate AND Steering IS Right"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS NegativeSmall AND DistanceToObstacle IS Far THEN Speed IS ForwardModerate AND Steering IS Left"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS PositiveLarge AND DistanceToObstacle IS Far THEN Speed IS ForwardSlow AND Steering IS Right"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS NegativeLarge AND DistanceToObstacle IS Far THEN Speed IS ForwardSlow AND Steering IS Left"));

        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS VeryNegative THEN Speed IS ReverseSlow AND Steering IS SharpLeft"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS NegativeLarge THEN Speed IS ReverseModerate AND Steering IS Left"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS NegativeSmall THEN Speed IS ReverseFast AND Steering IS Left"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS Zero THEN Speed IS ReverseFast AND Steering IS Straight"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS PositiveSmall THEN Speed IS ReverseFast AND Steering IS Right"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS PositiveLarge THEN Speed IS ReverseModerate AND Steering IS Right"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS VeryPositive THEN Speed IS ReverseSlow AND Steering IS SharpRight"));

        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS Zero AND DistanceToObstacle IS Medium THEN Speed IS ForwardModerate AND Steering IS Straight"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS PositiveSmall AND DistanceToObstacle IS Medium THEN Speed IS ForwardSlow AND Steering IS Right"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF AngleToParkingSpot IS NegativeSmall AND DistanceToObstacle IS Medium THEN Speed IS ForwardSlow AND Steering IS Left"));


    }
}
