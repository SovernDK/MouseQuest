using System.Collections;
using Atlas.DB;
using Combat;

namespace Atlas.Player
{
    public class CastSpellCommand : PlayerCommand
    {
        public string SpellId { get; set; }
        
        public CastSpellCommand(PlayerSystem player, string spellId) : base(player)
        {
            SpellId = spellId;
        }

        public override IEnumerator Execute()
        {
            Spell spell = Database.Instance.GetSpell(SpellId);
            // if(!spell.overworldUse) yield break;
            
            ConsoleProDebug.LogToFilter($"Spell (id:{SpellId}) cast in overworld", "SpellSystem");

            foreach(ActionType action in spell.actions)
            {
                Action a = ActionFactory
                        .Create(action)
                        .ExecuteAction(System.Battler, null);
            }

            // System.EffectsSystem.AddEffect(spell.overworldEffect.name, System.transform.position, Quaternion.identity);

            yield return base.Execute();
        }
    }
}
