using System.Collections;
using Atlas.Enums;
using Atlas.Systems;

public class AttributeChangeCommand : BattleCommand
{
    private int AttributeId { get; set; }
    private int Value { get; set; }

    public AttributeChangeCommand(AtlasBattleSystem system, Battler user, Battler target, int attributeId, int value) : base(system, user, target)
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