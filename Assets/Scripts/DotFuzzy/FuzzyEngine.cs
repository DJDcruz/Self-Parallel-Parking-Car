using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DotFuzzy
{
    public class FuzzyEngine
    {
        public LinguisticVariableCollection linguisticVariableCollection = new LinguisticVariableCollection();
        public string consequent = String.Empty;
        public FuzzyRuleCollection fuzzyRuleCollection = new FuzzyRuleCollection();
        private string filePath = String.Empty;

        // Private methods for internal fuzzy logic evaluation...
        private LinguisticVariable GetConsequent()
        {
            return this.linguisticVariableCollection.Find(this.consequent);
        }

        private double Parse(string text)
        {
            int counter = 0;
            int firstMatch = 0;

            if (!text.StartsWith("("))
            {
                string[] tokens = text.Split();
                return this.linguisticVariableCollection.Find(tokens[0]).Fuzzify(tokens[2]);
            }

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '(':
                        counter++;
                        if (counter == 1)
                            firstMatch = i;
                        break;

                    case ')':
                        counter--;
                        if ((counter == 0) && (i > 0))
                        {
                            string substring = text.Substring(firstMatch + 1, i - firstMatch - 1);
                            string substringBrackets = text.Substring(firstMatch, i - firstMatch + 1);
                            int length = substringBrackets.Length;
                            text = text.Replace(substringBrackets, Parse(substring).ToString());
                            i = i - (length - 1);
                        }
                        break;

                    default:
                        break;
                }
            }

            return Evaluate(text);
        }

        private double Evaluate(string text)
        {
            // Logic for evaluating the fuzzy logic based on string input.
            string[] tokens = text.Split();
            string connective = "";
            double value = 0;
            for (int i = 0; i <= ((tokens.Length / 2) + 1); i = i + 2)
            {
                double tokenValue = Convert.ToDouble(tokens[i]);
                switch (connective)
                {
                    case "AND":
                        if (tokenValue < value) value = tokenValue;
                        break;
                    case "OR":
                        if (tokenValue > value) value = tokenValue;
                        break;
                    default:
                        value = tokenValue;
                        break;
                }

                if ((i + 1) < tokens.Length)
                    connective = tokens[i + 1];
            }

            return value;
        }
        public LinguisticVariableCollection LinguisticVariableCollection
        {
            get { return linguisticVariableCollection; }
        }
        public FuzzyRuleCollection FuzzyRuleCollection
        {
            get { return fuzzyRuleCollection; }
        }

        public void Evaluate()
        {
            // Internal evaluation using fuzzy rules and defuzzify method
            Defuzzify(); // Assume Defuzzify computes the outputs
        }

        public double Defuzzify()
        {
            double numerator = 0;
            double denominator = 0;
            foreach (MembershipFunction membershipFunction in this.GetConsequent().MembershipFunctionCollection)
            {
                membershipFunction.Value = 0;
            }

            foreach (FuzzyRule fuzzyRule in this.fuzzyRuleCollection)
            {
                fuzzyRule.Value = Parse(fuzzyRule.Conditions());
                string[] tokens = fuzzyRule.Text.Split();
                MembershipFunction membershipFunction = this.GetConsequent().MembershipFunctionCollection.Find(tokens[tokens.Length - 1]);

                UnityEngine.Debug.Log($"Evaluating Rule: {fuzzyRule.Text} with Value: {fuzzyRule.Value}"); 


                if (fuzzyRule.Value > membershipFunction.Value)
                    membershipFunction.Value = fuzzyRule.Value;
            }

            foreach (MembershipFunction membershipFunction in this.GetConsequent().MembershipFunctionCollection)
            {
                numerator += membershipFunction.Centorid() * membershipFunction.Area();
                denominator += membershipFunction.Area();
            }

            return numerator / denominator;
        }

        // Getter and setter for the input/output values
        public void SetInput(string variableName, double value)
        {
            LinguisticVariable variable = linguisticVariableCollection.Find(variableName);
            if (variable != null)
            {
                variable.inputValue = value;
            }
        }

        public double GetOutput(string variableName)
        {
            LinguisticVariable variable = linguisticVariableCollection.Find(variableName);
            if (variable != null)
            {
                return Defuzzify(); // Or return specific output logic based on the fuzzy rules
            }
            return 0;
        }

    }
}
