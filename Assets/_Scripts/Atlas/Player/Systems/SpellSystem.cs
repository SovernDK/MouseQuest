using Atlas.Core;
using Atlas.DB;
using Atlas.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class SpellSystem : MonoBehaviour, ISystem
{
    private SpellModel _spellModel;

    [Inject]
    private SpellPresenter Presenter { get; set; }
    
    [Inject]
    private PlayerSystem Player { get; set; }

    public UnityEvent<string> spellCasted;

    public SpellModel SpellModel { get => _spellModel; set => _spellModel = value; }

    private void Awake() 
    {
        Presenter.System = this;

        _spellModel = new SpellModel();
    }

    private void Start() 
    {
        PlayerCharacter _playerCharacter = Player.PlayerCharacter;

        _playerCharacter.startingSpellbook.ForEach(spellPrototype =>
        {
            if(spellPrototype != null)
                _spellModel.Unlock(spellPrototype.spell.name);
        });
        
        _spellModel.FillAllSpells();
        Presenter.ApplySpells(_spellModel.UnlockedSpells);
    }
    
    public void Toggle()
    {
        Presenter.Toogle();
    }

    public void Hide()
    {
        Presenter.Hide();
    }

    public void Show()
    {
        Presenter.Show();
    }

    public void Refresh()
    {
        Presenter.ApplySpells(_spellModel.UnlockedSpells);
    }

    public void DecreaseSpellAmount(string spellId, int amount = 1)
    {
        SpellModel.DecreaseSpellAmount(spellId, amount);
        Presenter.ApplySpells(SpellModel.UnlockedSpells);
    }

    public void DecreaseSpellAmountByOne(string spellId)
    {
        SpellModel.DecreaseSpellAmount(spellId, 1);
        Presenter.ApplySpells(SpellModel.UnlockedSpells);
    }

    public void IncreaseSpellAmount(string spellId, int amount = 1)
    {
        SpellModel.IncreaseSpellAmount(spellId, amount);
        Presenter.ApplySpells(SpellModel.UnlockedSpells);
    }

    public void EnableSpells(bool enabled)
    {
        Presenter.EnableSpells(enabled);
    }

    public void UnlockSpell(string spellId)
    {
        SpellModel.Unlock(spellId);
        Refresh();
    }

    public void CastSpell(string spellId)
    {
        // DecreaseSpellAmount(spellId, 1);
        spellCasted?.Invoke(spellId);
        Presenter.ApplySpells(_spellModel.UnlockedSpells);
    }

    public void Initialize()
    {
    }
}
