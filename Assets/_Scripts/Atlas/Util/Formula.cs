using System;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Formula
{
    [ValidateInput("ValidateExpression", "$expression", InfoMessageType.Error)]
    [Multiline]
    [HideLabel]
    public string expression;

    public Formula()
    {

    }

    public Formula(string _expression)
    {
        expression = _expression;
    }

    public float Parse(Battler a, Battler b)
    {
        Debug.Log($"expression user {a.name}");
        string pattern = @"\b(a|b)\.(\w+)\b";

        string processedExpression = Regex.Replace(expression, pattern, match =>
        {
            string battlerReference = match.Groups[1].Value;
            string propertyName = match.Groups[2].Value;

            Battler actor = battlerReference.Equals("a") ? a : b;
            float propertyValue = actor.GetPropertyValue(propertyName);
            
            return propertyValue.ToString();
        });

        GRES_Solver solver = new GRES_Solver();
        solver.SetExpression(processedExpression);
        solver.Prepare();

        return solver.Evaluate();
    }

    private bool ValidateExpression(string expression, ref string errorMessage)
    {
        // Check for null or empty string
        if (string.IsNullOrEmpty(expression))
        {
            errorMessage = "Expression cannot be empty.";
            return false;
        }

        // Basic syntax validation pattern
        // This pattern might need adjustments based on your actual syntax rules
        string validCharsPattern = @"^[\w\s\.\+\-\*\/\(\)a-b]+$";
        if (!Regex.IsMatch(expression, validCharsPattern))
        {
            errorMessage = "Expression contains invalid characters.";
            return false;
        }

        // Check for invalid property references (simple check)
        // string propertyRefPattern = @"\b(a|b)\.(\w+)\b";
        // if (!Regex.IsMatch(expression, propertyRefPattern))
        // {
        //     errorMessage = "Expression must reference at least one actor property.";
        //     return false;
        // }

        return true;
    }
}
