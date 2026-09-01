using Atlas.UI;
using Atlas.Views;
using CharacterSheet;
using UnityEngine;

public class AttributePresenter : IPresenter<AttributeSystem, AttributeView>
{
    public AttributeSystem System { get; set; }
    public AttributeView View { get; set; }

    public void ApplyAttributes(Attribute[] attributes)
    {
        View.ApplyAttributes(attributes);
    }

    public void ApplyResistances(Resistance[] resistances)
    {
        View.ApplyResistances(resistances);
    }

    public void ApplyLevel(LevelModel level)
    {
        View.ApplyLevel(level);
    }
    
    public void Show()
    {
        View.Show();
    }

    public void Hide()
    {
        View.Hide();
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
