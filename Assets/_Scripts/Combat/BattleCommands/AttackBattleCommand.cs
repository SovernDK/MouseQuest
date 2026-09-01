using System.Collections;
using Atlas.Enums;
using Atlas.Systems;
using Atlas.Utility;
using Combat;
using I2.Loc;
using UnityEngine;

public class AttackBattleCommand : BattleCommand
{
    public AttackBattleCommand(AtlasBattleSystem system, Battler user, Battler target) : base(system, user, target) 
    { 
        Name = LocalizationManager.GetTranslation("ui_atk_command");
    }

    public override IEnumerator Execute()
    {
        if(Random.Range(1, 100) < Config.Instance.baseAttackHitChance)
        {
            DamageAction action = User.GetAttackAction();

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
            yield return System.ShowNotification($"{User.name} Attacks {Target.name}!");
        }
        else
        {
            ParticleEffect fail = new ParticleEffect(Config.Instance.defaultFailEffect.name, Target.BattlerTransform.position)
                                .SetDelay(0f)
                                .Build() as ParticleEffect;
            System.Effects.AddEffect(fail);

            yield return System.ShowNotification($"{User.name} Attacks Failed!");
        }
    
        yield return base.Execute();
    }
}

public class AttackCommandLog : BattleLogEntry
{
    Battler Target { get; set; }
    int Damage { get; set; }

    public AttackCommandLog(Battler target, int damage)
    {
        Target = target;
        Damage = damage;
    }

    public override string GetLogValue()
    {
        return "<b>" + Target.Name + "</b> " + LocalizationManager.GetTranslation("battle_have_been_hit_for") + " " + Damage + " " + LocalizationManager.GetTranslation("battle_damage");
    }
}