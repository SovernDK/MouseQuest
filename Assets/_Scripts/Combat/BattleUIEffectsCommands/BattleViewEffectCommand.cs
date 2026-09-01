using System.Collections;

public abstract class BattleViewEffectCommand
{
    protected BattleSystem System { get; set; }
    protected Battler User { get; set; }
    protected Battler Target { get; set; }

    protected BattleViewEffectCommand(BattleSystem system, Battler user, Battler target)
    {
        System = system;
        User = user;
        Target = target;
    }

    protected BattleViewEffectCommand(Battler user, Battler target)
    {
        User = user;
        Target = target;
    }

    public virtual IEnumerator Execute()
    {
        yield return null;
    }
}
