using System.Collections;
using Atlas.Enums;
using Atlas.Systems;
using UnityEngine;
public class AttributeMaxChangeCommand : BattleCommand
{
    private int AttributeId { get; set; }
    private int Value { get; set; }

    public AttributeMaxChangeCommand(AtlasBattleSystem system, Battler user, Battler target, int attributeId, int value) : base(system, user, target)
    {
        AttributeId = attributeId;
        Value = value;
    }

    public override IEnumerator Execute()
    {
        // User.Attributes.IncreaseMaxAttribute((EAttribute) AttributeId, Value);
        User.Attributes.AddMaxModifier((EAttribute) AttributeId, "other", Value);
        yield return null;
    }
}