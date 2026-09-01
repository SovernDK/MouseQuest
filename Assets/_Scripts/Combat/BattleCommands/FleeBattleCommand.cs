using System.Collections;
using Atlas.Systems;

public class FleeBattleCommand : BattleCommand
{
    private int Value { get; set; }

    public FleeBattleCommand(Battler user, Battler target, AtlasBattleSystem battleSystem) : base(user, target)
    {
        System = battleSystem;
    }

    public override IEnumerator Execute()
    {
        // System.FleeBattle();
        yield return null;
    }
}