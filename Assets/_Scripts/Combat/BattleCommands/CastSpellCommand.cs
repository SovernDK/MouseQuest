using System.Collections;
using Combat;
using Atlas.DB;
using I2.Loc;
using Atlas.Systems;
using UnityEngine;

public class CastSpellCommand : BattleCommand
{
    private string SpellId { get; set; }

    public CastSpellCommand(AtlasBattleSystem system, Battler user, Battler target, string spellId) : base(system, user, target)
    {
        SpellId = spellId;
        Name = LocalizationManager.GetTranslation(Database.Instance.GetSpell(SpellId).name);
    }

    public override IEnumerator Execute()
    {
        Spell spell = Database.Instance.GetSpell(SpellId);
        
        if(Random.Range(1, 100) < spell.hitChance)
        {
            foreach(ActionType action in spell.actions)
            {
                //Repeats
                int amountOfHits = Random.Range(action.repeat.x, action.repeat.y+1);
                for(int i = 0; i < amountOfHits; i++)
                {
                    action.source = "spell_" + spell.name;
                    Vector3 targetOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0); 

                    Action a = ActionFactory
                                .Create(action)
                                .ExecuteAction(User, Target);

                    //What about not playing animation if action isn't damage

                    if(action.target == Atlas.Enums.ETarget.User)
                    {
                        if(a is DamageAction)
                        {
                            SoundEffect spellSfx = new SoundEffect(spell.targetSfxEffect.name, User.BattlerTransform.position + targetOffset)
                                .Build() as SoundEffect;
                            System.Effects.AddEffect(spellSfx);

                            ParticleEffect spellEffect = new ParticleEffect(spell.targetEffect.name, User.BattlerTransform.position + targetOffset)
                                .SetDelay(0f)
                                .Build() as ParticleEffect;
                            System.Effects.AddEffect(spellEffect);
                        }
                        yield return User.PlayAnimation(action.actionType, new AnimationData() { action = a });
                    }
                    else if(action.target == Atlas.Enums.ETarget.Other)
                    {
                        if(a is DamageAction)
                        {
                            SoundEffect spellSfx = new SoundEffect(spell.targetSfxEffect.name, User.BattlerTransform.position + targetOffset)
                                .Build() as SoundEffect;
                            System.Effects.AddEffect(spellSfx);

                            ParticleEffect spellEffect = new ParticleEffect(spell.targetEffect.name, Target.BattlerTransform.position + targetOffset)
                                .SetDelay(0f)
                                .Build() as ParticleEffect;
                            System.Effects.AddEffect(spellEffect);
                        }
                        
                        yield return Target.PlayAnimation(action.actionType, new AnimationData() { action = a });
                    }
                    else if(action.target == Atlas.Enums.ETarget.Both)
                    {
                        if(a is DamageAction)
                        {
                            SoundEffect spellSfx = new SoundEffect(spell.targetSfxEffect.name, User.BattlerTransform.position + targetOffset)
                                .SetDelay(.1f)
                                .Build() as SoundEffect;

                            SoundEffect spellSfx2 = new SoundEffect(spell.targetSfxEffect.name, User.BattlerTransform.position + (-targetOffset))
                                .SetDelay(.1f)
                                .Build() as SoundEffect;

                            ParticleEffect spellEffect = new ParticleEffect(spell.targetEffect.name, Target.BattlerTransform.position + targetOffset)
                                .SetDelay(0f)
                                .Build() as ParticleEffect;

                            ParticleEffect spellEffect2 = new ParticleEffect(spell.targetEffect.name, User.BattlerTransform.position + (-targetOffset))
                                .SetDelay(.1f)
                                .Build() as ParticleEffect;
                                
                            System.Effects.AddEffect(spellSfx);
                            System.Effects.AddEffect(spellSfx2);

                            System.Effects.AddEffect(spellEffect);
                            System.Effects.AddEffect(spellEffect2);
                        }

                        Target.PlayAnimation(action.actionType, new AnimationData() { action = a });
                        yield return Target.PlayAnimation(action.actionType, new AnimationData() { action = a });
                    }
                }
            }

            yield return System.ShowNotification($"{User.name} cast spell {Name}!");
        }
        else {}

        User.CastSpell(SpellId);
        yield return base.Execute();
    }
}

public class MagicAttackCommandLog : BattleLogEntry
{
    Battler Target { get; set; }
    int Damage { get; set; }

    public MagicAttackCommandLog(Battler target, int damage)
    {
        Target = target;
        Damage = damage;
    }

    public override string GetLogValue()
    {
        return "<b>" + Target.Name + "</b> " + LocalizationManager.GetTranslation("battle_have_been_hit_for") + " " + Damage + " " + LocalizationManager.GetTranslation("battle_magic_damage");
    }
}