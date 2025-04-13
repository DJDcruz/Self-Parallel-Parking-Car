using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine; // For UnityEngine.Debug.Log

namespace DotFuzzy
{
    #region LinguisticVariable and Collection

    /// <summary>
    /// Represents a linguistic variable.
    /// </summary>
    public class LinguisticVariable
    {
        private string name = String.Empty;
        private MembershipFunctionCollection membershipFunctionCollection = new MembershipFunctionCollection();
        public double InputValue = 0;

        public LinguisticVariable() { }

        public LinguisticVariable(string name)
        {
            this.Name = name;
        }

        public LinguisticVariable(string name, MembershipFunctionCollection membershipFunctionCollection)
        {
            this.Name = name;
            this.MembershipFunctionCollection = membershipFunctionCollection;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public MembershipFunctionCollection MembershipFunctionCollection
        {
            get { return membershipFunctionCollection; }
            set { membershipFunctionCollection = value; }
        }

        public double Input
        {
            get { return InputValue; }
            set { InputValue = value; }
        }

        /// <summary>
        /// Fuzzify the variable using a specified membership function.
        /// </summary>
        public double Fuzzify(string membershipFunctionName)
        {
            MembershipFunction membershipFunction = this.membershipFunctionCollection.Find(membershipFunctionName);

            if ((membershipFunction.X0 <= this.InputValue) && (this.InputValue < membershipFunction.X1))
                return (this.InputValue - membershipFunction.X0) / (membershipFunction.X1 - membershipFunction.X0);
            else if ((membershipFunction.X1 <= this.InputValue) && (this.InputValue <= membershipFunction.X2))
                return 1;
            else if ((membershipFunction.X2 < this.InputValue) && (this.InputValue <= membershipFunction.X3))
                return (membershipFunction.X3 - this.InputValue) / (membershipFunction.X3 - membershipFunction.X2);
            else
                return 0;
        }

        public double MinValue()
        {
            double minValue = this.membershipFunctionCollection[0].X0;
            foreach (var mf in this.membershipFunctionCollection)
            {
                if (mf.X0 < minValue)
                    minValue = mf.X0;
            }
            return minValue;
        }

        public double MaxValue()
        {
            double maxValue = this.membershipFunctionCollection[0].X3;
            foreach (var mf in this.membershipFunctionCollection)
            {
                if (mf.X3 > maxValue)
                    maxValue = mf.X3;
            }
            return maxValue;
        }

        public double Range()
        {
            return this.MaxValue() - this.MinValue();
        }
    }

    /// <summary>
    /// A collection of linguistic variables.
    /// </summary>
    public class LinguisticVariableCollection : IEnumerable<LinguisticVariable>
    {
        private List<LinguisticVariable> variables = new List<LinguisticVariable>();

        public void Add(LinguisticVariable variable)
        {
            variables.Add(variable);
        }

        public IEnumerator<LinguisticVariable> GetEnumerator()
        {
            return variables.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public LinguisticVariable Find(string linguisticVariableName)
        {
            string trimmedName = linguisticVariableName.Trim();
            foreach (LinguisticVariable variable in variables)
            {
                if (variable.Name.Trim().Equals(trimmedName, StringComparison.OrdinalIgnoreCase))
                {
                    return variable;
                }
            }
            throw new Exception("LinguisticVariable not found: " + linguisticVariableName);
        }
    }

    #endregion

    #region Membership Functions and Collection

    /// <summary>
    /// Represents a membership function.
    /// </summary>
    public class MembershipFunction
    {
        private string name = String.Empty;
        private double x0 = 0;
        private double x1 = 0;
        private double x2 = 0;
        private double x3 = 0;
        private double value = 0;

        public MembershipFunction() { }

        public MembershipFunction(string name)
        {
            this.Name = name;
        }

        public MembershipFunction(string name, double x0, double x1, double x2, double x3)
        {
            this.Name = name;
            this.X0 = x0;
            this.X1 = x1;
            this.X2 = x2;
            this.X3 = x3;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double X0
        {
            get { return x0; }
            set { x0 = value; }
        }

        public double X1
        {
            get { return x1; }
            set { x1 = value; }
        }

        public double X2
        {
            get { return x2; }
            set { x2 = value; }
        }

        public double X3
        {
            get { return x3; }
            set { x3 = value; }
        }

        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Calculates the centroid of the trapezoidal membership function.
        /// </summary>
        public double Centorid()
        {
            double a = this.x2 - this.x1;
            double b = this.x3 - this.x0;
            double c = this.x1 - this.x0;
            return ((2 * a * c) + (a * a) + (c * b) + (a * b) + (b * b)) / (3 * (a + b)) + this.x0;
        }

        /// <summary>
        /// Calculates the area of the membership function.
        /// </summary>
        public double Area()
        {
            double a = this.Centorid() - this.x0;
            double b = this.x3 - this.x0;
            return (this.value * (b + (b - (a * this.value)))) / 2;
        }
    }

    /// <summary>
    /// A collection of membership functions.
    /// </summary>
    public class MembershipFunctionCollection : Collection<MembershipFunction>
    {
        public MembershipFunction Find(string membershipFunctionName)
        {
            foreach (MembershipFunction function in this)
            {
                if (function.Name.Equals(membershipFunctionName, StringComparison.OrdinalIgnoreCase))
                {
                    return function;
                }
            }
            throw new Exception("MembershipFunction not found: " + membershipFunctionName);
        }
    }

    #endregion

    #region Fuzzy Rule and Collection

    /// <summary>
    /// Represents a fuzzy rule with support for multiple output variables.
    /// Rule format example:
    /// "IF Distance IS Close AND Angle IS Acute THEN Speed IS Slow AND Steering IS Sharp"
    /// </summary>
    public class FuzzyRule
    {
        // The antecedents string (conditions)
        public string Antecedents { get; set; }
        // A mapping from output variable names to their membership function names.
        public Dictionary<string, string> Consequents { get; set; }
        // The firing strength of the rule after evaluation.
        public double Value { get; set; }

        public FuzzyRule(string ruleText)
        {
            ParseRuleText(ruleText);
        }

        private void ParseRuleText(string ruleText)
        {
            // Split into IF/THEN parts.
            string[] parts = ruleText.Split(new string[] { "THEN" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new Exception("Invalid rule format. Expected one 'THEN'.");
            
            // Remove "IF" from the antecedents part.
            string antecedentsPart = parts[0].Trim();
            if (!antecedentsPart.StartsWith("IF", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Rule must start with 'IF'.");
            this.Antecedents = antecedentsPart.Substring(2).Trim();

            // Process consequents.
            this.Consequents = new Dictionary<string, string>();
            string consequentsPart = parts[1].Trim();
            // Split multiple outputs by the "AND" keyword.
            string[] outputs = consequentsPart.Split(new string[] { "AND" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var output in outputs)
            {
                // Expected format: Variable IS MembershipFunctionName
                string trimmedOutput = output.Trim();
                string[] tokens = trimmedOutput.Split();
                if (tokens.Length != 3 || !tokens[1].Equals("IS", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Invalid consequent format: " + trimmedOutput);
                string variableName = tokens[0].Trim();
                string membershipFunctionName = tokens[2].Trim();
                Consequents[variableName] = membershipFunctionName;
            }
        }
    }

    /// <summary>
    /// A collection of fuzzy rules.
    /// </summary>
    public class FuzzyRuleCollection : Collection<FuzzyRule>
    {
    }

    #endregion

    #region Fuzzy Engine (Extended for Multiple Inputs and Outputs)

    /// <summary>
    /// The fuzzy engine that processes multiple input and output variables.
    /// </summary>
    public class FuzzyEngine
    {
        // Separate collections for inputs and outputs.
        public LinguisticVariableCollection InputVariableCollection { get; private set; }
        public LinguisticVariableCollection OutputVariableCollection { get; private set; }
        public FuzzyRuleCollection FuzzyRuleCollection { get; private set; }

        public FuzzyEngine()
        {
            InputVariableCollection = new LinguisticVariableCollection();
            OutputVariableCollection = new LinguisticVariableCollection();
            FuzzyRuleCollection = new FuzzyRuleCollection();
        }

        /// <summary>
        /// Sets the input value for a given variable.
        /// </summary>
        public void SetInput(string variableName, double value)
        {
            LinguisticVariable variable = InputVariableCollection.Find(variableName);
            variable.InputValue = value;
        }

        /// <summary>
        /// Gets the crisp output for a given output variable.
        /// </summary>
        public double GetOutput(string variableName)
        {
            LinguisticVariable outputVar = OutputVariableCollection.Find(variableName);
            return Defuzzify(outputVar);
        }

        /// <summary>
        /// Evaluate the fuzzy rules and update the output membership functions.
        /// </summary>
        public void Evaluate()
        {
            // Reset all output membership functions.
            foreach (LinguisticVariable outputVar in OutputVariableCollection)
            {
                foreach (MembershipFunction mf in outputVar.MembershipFunctionCollection)
                {
                    mf.Value = 0;
                }
            }

            // Evaluate each rule.
            foreach (FuzzyRule rule in FuzzyRuleCollection)
            {
                double ruleStrength = Parse(rule.Antecedents);

                // For each output specified in the rule, update the corresponding membership function.
                foreach (var kvp in rule.Consequents)
                {
                    string outputVarName = kvp.Key;
                    string mfName = kvp.Value;

                    LinguisticVariable outputVar = OutputVariableCollection.Find(outputVarName);
                    MembershipFunction mf = outputVar.MembershipFunctionCollection.Find(mfName);

                    // Use maximum (max aggregation) for overlapping rules.
                    if (ruleStrength > mf.Value)
                    {
                        mf.Value = ruleStrength;
                    }
                }
            }

            foreach (FuzzyRule rule in FuzzyRuleCollection)
            {
                double ruleStrength = Parse(rule.Antecedents);
                Debug.Log($"Rule fired: {rule.Antecedents} => Strength={ruleStrength}");
                
            }

            // Optionally, log crisp outputs for each output variable.
            foreach (LinguisticVariable outputVar in OutputVariableCollection)
            {
                double crispOutput = Defuzzify(outputVar);
                Debug.Log($"{outputVar.Name} output: {crispOutput}");
            }
        }

        /// <summary>
        /// Defuzzify an output variable using its membership functions.
        /// </summary>
        private double Defuzzify(LinguisticVariable outputVar)
        {
            double numerator = 0;
            double denominator = 0;
            foreach (MembershipFunction mf in outputVar.MembershipFunctionCollection)
            {
                double centroid = mf.Centorid();
                double area = mf.Area();
                numerator += centroid * area;
                denominator += area;
            }
            Debug.Log($"Defuzzify {outputVar.Name}: numerator={numerator}, denominator={denominator}");

            if (denominator == 0)
                return 0;
            return numerator / denominator;
        }

        /// <summary>
        /// Parses the antecedent expression and returns a firing strength.
        /// Supports nested expressions using parentheses.
        /// </summary>
        private double Parse(string text)
        {
            // If the text does not start with a parenthesis, it is a simple expression like "Distance IS Close"
            if (!text.Trim().StartsWith("("))
            {
                string[] tokens = text.Split();
                // Assume the format: Variable IS MembershipFunctionName
                LinguisticVariable inputVar = InputVariableCollection.Find(tokens[0]);
                return inputVar.Fuzzify(tokens[2]);
            }

            // Handle nested expressions.
            int counter = 0;
            int firstMatch = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '(')
                {
                    counter++;
                    if (counter == 1)
                        firstMatch = i;
                }
                else if (text[i] == ')')
                {
                    counter--;
                    if (counter == 0)
                    {
                        string innerExpression = text.Substring(firstMatch + 1, i - firstMatch - 1);
                        string substringWithBrackets = text.Substring(firstMatch, i - firstMatch + 1);
                        double innerValue = Parse(innerExpression);
                        text = text.Replace(substringWithBrackets, innerValue.ToString());
                        // Restart parsing as the text has changed.
                        return Parse(text);
                    }
                }
            }

            // Evaluate simple expression with connectives.
            return Evaluate(text);
        }

        /// <summary>
        /// Evaluates a simple fuzzy expression containing numeric values and connectives AND/OR.
        /// </summary>
        private double Evaluate(string text)
        {
            string[] tokens = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string connective = "";
            double value = 0;
            for (int i = 0; i < tokens.Length; i += 2)
            {
                double tokenValue = Convert.ToDouble(tokens[i]);
                if (string.IsNullOrEmpty(connective))
                {
                    value = tokenValue;
                }
                else if (connective.Equals("AND", StringComparison.OrdinalIgnoreCase))
                {
                    value = Math.Min(value, tokenValue);
                }
                else if (connective.Equals("OR", StringComparison.OrdinalIgnoreCase))
                {
                    value = Math.Max(value, tokenValue);
                }
                if ((i + 1) < tokens.Length)
                {
                    connective = tokens[i + 1];
                }
            }
            return value;
        }
    }
}
    #endregion