using System.Collections;
using Combat;
using UnityEngine;
using I2.Loc;
using Atlas.Core;

public class AttackEffectCommand : BattleViewEffectCommand
{
    public AttackEffectCommand(BattleSystem system, Battler user, Battler target) : base(system, user, target)
    {
    }

    public override IEnumerator Execute()
    {
        // System.EffectsSystem.AddCanvasEffect(User.BasicAttackEffect, Target.BattlerPosition, Quaternion.identity, 0f, Target.BattlerTransform);
        // System.EffectsSystem.AddSoundEffect(Gamemaster.Instance.Config.SlashSfx.name, Target.BattlerPosition, Quaternion.identity, .1f);

        // yield return User.PlayAnimation(EBattlerAnimation.Attack);
        // // yield return System.Shake(action.FinalDamageRatio * 10);
        // yield return System.Shake(10);
        // yield return Target.PlayAnimation(EBattlerAnimation.GetHit);
        yield return null;
    }
}
