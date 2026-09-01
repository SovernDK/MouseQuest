using Atlas.Enums;
using CharacterSheet;
using UnityEngine;

public static class Formulas
{
    public static int CalculateAttack(AttributesModel user, AttributesModel other)
    {
        int value = user.GetValue(EAttribute.Attack) - other.GetValue(EAttribute.Defence) / 2;
        return Mathf.Clamp(value, 1, int.MaxValue);
    }
}