namespace Combat
{
    public abstract class Action
    {
        public BattleLogEntry ActionLogEntry { get; set; }

        public Action() {}
        public virtual Action ExecuteAction(Battler user, Battler target, bool critical = false) { return this; }
    }
}
