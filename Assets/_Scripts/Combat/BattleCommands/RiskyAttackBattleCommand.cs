using System.Collections;
using Atlas.Enums;
using Atlas.Systems;
using Atlas.Utility;
using Combat;
using I2.Loc;
using UnityEngine;

public class RiskyAttackBattleCommand : BattleCommand
{
    public RiskyAttackBattleCommand(AtlasBattleSystem system, Battler user, Battler target) : base(system, user, target) { 
        Name = LocalizationManager.GetTranslation("ui_ris_atk_command");
    }

    public override IEnumerator Execute()
    {
        if(Random.Range(1, 100) < 95)
        {
            DamageAction action = User.GetRiskyAttackAction();

            ConsoleProDebug.LogToFilter($"Target {Target.name}, {User.BasicAttackEffect}", "BattleSystem");
            bool critical = Random.Range(0f, 1f) < (User.Attributes.GetValue(EAttribute.CriticalHit) / 100);
        
            action.ExecuteAction(User, Target, critical);

            SoundEffect sfx = new SoundEffect(User.BasicAttackSfx, User.BattlerTransform.position)
                            .Build() as SoundEffect;

            ParticleEffect attack = new ParticleEffect(User.BasicAttackEffect, Target.BattlerTransform.position)
                                .SetDelay(0f)
                                .Build() as ParticleEffect;

            System.Effects.AddEffect(sfx);
            System.Effects.AddEffect(attack);
            
            yield return Target.PlayAnimation(Atlas.DB.EActionType.Damage, new AnimationData() { duration = 1f, action = action });
            
            Formula formula = new Formula(Config.Instance.riskyAttackDebuffFormula);
            AddModifierAction defenceAction = new AddModifierAction(ETarget.User, formula, (int) EAttribute.Defence, "risky_attack", true, Config.Instance.riskyAttackDebuffTime);
            defenceAction.ExecuteAction(User, Target, critical);

            yield return System.ShowNotification($"{User.name} Attacks {Target.name}!");
        }
        else
        {
            
        }
    
        yield return base.Execute();
    }
}