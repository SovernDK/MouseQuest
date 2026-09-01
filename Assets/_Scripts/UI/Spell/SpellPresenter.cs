using Atlas.UI;

public class SpellPresenter : IPresenter<SpellSystem, SpellView>
{
    public SpellSystem System { get; set; }
    public SpellView View { get; set; }

    public void ApplySpells(SpellEntry[] spells)
    {
        View.ApplySpells(spells);
    }

    public void CastSpell(string spellId)
    {
        System.CastSpell(spellId);
    }

    public void Refresh()
    {

    }

    public void EnableSpells(bool enabled)
    {
        View.EnableSpells(enabled);
    }

    public void Hide()
    {
        View.Hide();
    }

    public void Show()
    {
        View.Show();
    }

    public void Toogle()
    {
        if(View.Visible)
        {
            View.Hide();
        }
        else
        {
            View.Show();
        }
    }
}
