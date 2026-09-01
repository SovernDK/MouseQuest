using System.Collections;
using CharacterSheet;
using Combat;
using UnityEngine;
using Zenject;
using Atlas.Battle;
using Atlas.Player;
using Atlas.Enums;
using Atlas.DB;
using Atlas.Utility;

public class PlayerBattler : Battler
{
    public override string Name => "Player";
    public override string BasicAttackEffect { get => Config.Instance.defaultAttackEffect.name; }
    public override string BasicAttackSfx { get => Config.Instance.SlashSfx.name; }
    public override EBattleAnchorSide Side { get => EBattleAnchorSide.Right; }
    public override AttributesModel Attributes { get => Player.GetComponent<AttributeSystem>().Attributes; }
    public override SpellModel Spellbook { get => Player.GetComponent<SpellSystem>().SpellModel; }
    public override EElement BaseAtkElement { get => Player.PlayerCharacter.baseElement; }

    public EquipmentModel Equipment { get => Player.GetComponent<InventorySystem>().EquipmentModel; }

    //Recently changed may cause problems
    [Inject]
    public PlayerSystem Player { get; set; }

    public void Initialize(PlayerSystem playerSystem)
    {
        base.Initialize();

        Player = playerSystem;
    }

    public override void OnBattleStart()
    {
        base.OnBattleStart();

        // foreach(string itemId in Equipment.GetAll())
        // {
        //     Item item = Database.Instance.GetItem(itemId);
        //     if(!item.IsEmpty() && item.itemType == EItemType.Weapon)
        //     {
        //         ConsoleProDebug.LogToFilter($"{item.formula.expression}", "InventorySystem");
        //         BaseAtkFormula = item.formula;
        //         Attributes.BaseElement = item.element;
        //     }
        // }
    }

    public override void CastSpell(string spellId)
    {
        // Spellbook.DecreaseSpellAmount(spellId, 1);
        OnSpellCast.Invoke(spellId);
    }

    public override DamageAction GetAttackAction()
    {   
        // if(!Equipment.Get(EEquipmentSlot.Weapon).Equals("item_empty"))
        // {
        //     Item weapon = Database.Instance.GetItem(Equipment.Get(EEquipmentSlot.Weapon));
        //     return new DamageAction(ETarget.Other, weapon.formula, weapon.element);
        // }
        return new DamageAction(ETarget.Other, BaseAtkFormula, BaseAtkElement);
    }

    public override DamageAction GetRiskyAttackAction()
    {
        return new DamageAction(ETarget.Other, RiskyAtkFormula, BaseAtkElement);
    }

    public void ChangeBaseAtkFromEquipment(string[] equipments)
    {
        // foreach(string itemId in equipments)
        // {
        //     Item item = Database.Instance.GetItem(itemId);
        //     if(item.itemType == EItemType.Weapon)
        //     {
        //         BaseAtkFormula = item.formula;
        //         BaseAtkElement = item.element;
        //     }
        // }
    }

    protected override IEnumerator PlayGetHit(float duration, float value)
    {
        SoundEffect sfx = new SoundEffect(Config.Instance.GetHitSfx.name, _battlerSprite.transform.position)
                            .Build() as SoundEffect;

        _effects.AddEffect(sfx);
        return base.PlayGetHit(duration, value);
    }

    public override IEnumerator PlayAttack()
    {
        yield return new WaitForSeconds(.5f);
    }

    private void Dress(SpriteRenderer renderer, Sprite sprite)
    {
        renderer.sprite = sprite;
    }

    public void ClearQuickSlot()
    {
        AddQuickCommand(null);
    } 
}
