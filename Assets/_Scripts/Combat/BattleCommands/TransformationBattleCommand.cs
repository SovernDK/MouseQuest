using System.Collections;
using Atlas.DB;
using Atlas.Systems;

public class TransformationBattleCommand : BattleCommand
{
    Enemy _enemy;
    bool _transferHp;

    public TransformationBattleCommand(AtlasBattleSystem system, Battler user, Battler target, Enemy enemy, bool transferHp) : base(system, user, target)
    {
        _enemy = enemy;
        _transferHp = transferHp;
    }

    public override IEnumerator Execute()
    {
        System.Transformation(_enemy, _transferHp);
        yield return System.ShowNotification($"{User.name} transforms!");
    }
}