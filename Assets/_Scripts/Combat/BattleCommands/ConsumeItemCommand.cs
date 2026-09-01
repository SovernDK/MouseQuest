using System.Collections;
using Combat;
using Atlas.DB;
using Atlas.Systems;
using I2.Loc;

public class ConsumeItemCommand : BattleCommand
{
    private string ItemId { get; set; }

    public ConsumeItemCommand(AtlasBattleSystem system, Battler user, Battler target, string itemId) : base(system, user, target)
    {
        ItemId = itemId;
        Name = LocalizationManager.GetTranslation(Database.Instance.GetItem(itemId).name);
    }

    public override IEnumerator Execute()
    {
        Item item = Database.Instance.GetItem(ItemId);

        foreach(ActionType action in item.actions)
        {
            for(int i = 0; i < action.repeat.y; i++)
            {
                action.source = "item_" + item.name;
                
                Action a = ActionFactory
                            .Create(action)
                            .ExecuteAction(User, Target);

                if(action.target == Atlas.Enums.ETarget.User)
                {
                    ParticleEffect spellEffect = new ParticleEffect(item.effect.name, User.BattlerTransform.position)
                        .SetDelay(0f)
                        .Build() as ParticleEffect;
                    System.Effects.AddEffect(spellEffect);
                    yield return User.PlayAnimation(action.actionType, new AnimationData() { action = a });
                }
                else if(action.target == Atlas.Enums.ETarget.Other)
                {
                    ParticleEffect spellEffect = new ParticleEffect(item.effect.name, Target.BattlerTransform.position)
                        .SetDelay(0f)
                        .Build() as ParticleEffect;
                    System.Effects.AddEffect(spellEffect);
                    
                    yield return Target.PlayAnimation(action.actionType, new AnimationData() { action = a });
                }
            }

            yield return System.ShowNotification($"{User.name} consumes item {item.name}!");
        }

        yield return null;
    }
}