using System.Collections;
using Atlas.DB;
using Atlas.Enums;

namespace Atlas.Player
{
   public class ConsumeItemCommand : PlayerCommand
    {
        private int ItemId { get; set; }
        public ConsumeItemCommand(PlayerSystem player, int itemId) : base(player)
        {
            ItemId = itemId;
        }

        public override IEnumerator Execute()
        {
            // Item item = Database.Instance.GetItem(ItemId);
            // foreach(ActionType action in item.actions)
            // {
            //     int[] args = new int[5];
            //     args[0] = (int) ETarget.User;
            //     args[1] = action.value;
            //     args[2] = 0;
            //     args[3] = (int) action.attribute;
            //     args[4] = action.turnLimitCount;

                // System.ActionSystem
                //     .GetAction((int) action.actionType, args)
                //     .ExecuteAction(System.Battler, System.Battler);
            // }

            yield return base.Execute();
        }
    }
}
