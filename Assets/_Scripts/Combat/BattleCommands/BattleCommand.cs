using System;
using System.Collections;
using Atlas.Systems;

public abstract class BattleCommand
{
    protected AtlasBattleSystem System { get; set; }
    protected Battler User { get; set; }
    protected Battler Target { get; set; }
    public string Name { get; set; }

    protected BattleCommand(AtlasBattleSystem system, Battler user, Battler target)
    {
        System = system;
        User = user;
        Target = target;
    }

    protected BattleCommand(Battler user, Battler target)
    {
        User = user;
        Target = target;
    }

    public virtual IEnumerator Execute()
    {
        yield return null;
    }

    public virtual IEnumerator Failure()
    {
        yield return null;
    }

    public virtual IEnumerator Success()
    {
        yield return null;
    }
}
