using System.Collections;

namespace Atlas.Player
{
    public abstract class PlayerCommand
    {
        protected PlayerSystem System { get; set; }

        protected PlayerCommand(PlayerSystem player)
        {
            System = player;
        }

        public virtual IEnumerator Execute()
        {
            yield return null;
        }
    }
}
