using System.Collections;
using Atlas.Enums;
using UnityEngine;

public class HealBattleCommand : BattleCommand
{
    private int AttributeId { get; set; }
    private int Value { get; set; }

    public HealBattleCommand(Battler user, Battler target, int attributeId, int value) : base(user, target)
    {
        AttributeId = attributeId;
        Value = value;
    }

    public override IEnumerator Execute()
    {
        User.Attributes.IncreaseAttribute((EAttribute) AttributeId, Value);
        yield return null;
    }
}