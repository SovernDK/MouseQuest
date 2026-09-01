using System.Collections;
using Atlas.DB;
using ModestTree;
using UnityEngine;

public class CastSpellEffectCommand : BattleViewEffectCommand
{
    private int SpellId { get; set; }
    public CastSpellEffectCommand(BattleSystem system, Battler user, Battler target, int spellId) : base(system, user, target)
    {
        SpellId = spellId;
    }

    public override IEnumerator Execute()
    {
        // Spell spell = Database.Instance.GetSpell(SpellId);

        // if(!spell.effectName.IsEmpty())
        // {
        //     // System.EffectsSystem.AddCanvasEffect(spell.effectName, Target.EffectsAnchor, Quaternion.identity, 0f);
        //     // System.EffectsSystem.AddSoundEffect(spell.effectName, Target.EffectsAnchor.position, Quaternion.identity, 0f);
        // }

        // foreach(ActionType action in spell.actions)
        // {
        //     yield return System.Shake(10);
        //     yield return Target.PlayAnimation(System.ActionSystem.GetAnimation((int) action.actionType));
        // }
        yield return new WaitForSeconds(0f);
    }
}
