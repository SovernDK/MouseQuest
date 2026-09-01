using System.Collections;
using System.Collections.Generic;
using Atlas.DB;
using Atlas.UI;
using CharacterSheet;
using Combat;
using UnityEngine;

public class BattlePresenter : IPresenter<BattleSystem, BattleView>
{
    public BattleSystem System { get; set; }
    public BattleView View { get; set; }

    public AttributesModel PlayerAttributes => System.Player.AttributeSystem.Attributes;

    public void StartBattle(Battle battle, Battler player, Atlas.DB.Enemy enemy)
    {
        View.SetBattleResultVisibility(false);
        View.SetBattleLootVisibility(false);
        View.SetCommandsVisibility(false);
        View.SetBattlersVisibility(true);
        View.EnableEndButton(false);
        View.SetHPVisibility(false);

        View.SetPlayerAttributes(player.Attributes);
        View.SetEnemyName(enemy.name);

        View.Show();
    }

    public void FadeInHP()
    {
        View.FadeInHP();
    }

    public void ApplyAttributes(AttributesModel attributes)
    {
        View.SetPlayerAttributes(attributes);
    }

    public void SetBattleLogVisibility(bool visible)
    {
        View.SetBattleLogVisibility(visible);
    }

    public void ClearBattleLog()
    {
        View.ClearBattleLog();
    }

    public IEnumerator EnableCommands(bool enabled)
    {
        yield return View.EnableCommands(enabled);
    }

    public void SetCommandsVisibility(bool visible)
    {
        View.SetCommandsVisibility(visible);
    }

    public void SetEnemyLabelVisibility(bool visible)
    {
        View.SetEnemyLabelVisibility(visible);
    }

    public void SetBattleResultVisibility(bool visible)
    {
        View.SetBattleResultVisibility(visible);
    }

    public void ApplyInventory(InventorySlot[] slots)
    {
        View.ApplyInventory(slots);
    }

    public void ApplySpells(SpellEntry[] spells)
    {
        View.ApplySpells(spells);
    }

    public void UpdatePlayerAttributes(Battler player)
    {
        View.SetPlayerAttributes(player.Attributes);
    }

    public void UpdateBattleLog(BattleLogEntry entry)
    {
        View.SetLogEntry(entry);
    }

    public void CastSpell(int spellId)
    {
        // System.AddSpellCommand(spellId);
    }

    public void SetInventoryVisibility(bool visible)
    {
        View.SetInventoryVisibility(visible);
    }

    public void SetSpellsVisibility(bool visible)
    {
        View.SetSpellsVisibility(visible);
    }

    public void SetLevelUpVisibility(bool visible)
    {
        View.SetLevelUpVisibility(visible);
    }
    
    public void SetEndButtonVisibility(bool visible)
    {
        View.SetEndButtonVisibility(visible);
    } 

    public void SetEndButtonEnabled(bool enable)
    {
        View.EnableEndButton(enable);
    } 

    public void SetBattleLootVisibility(bool visible)
    {
        View.SetBattleLootVisibility(visible);
    }

    #region Animation 
    public IEnumerator PlayNormalAttack(DamageAction action, Battler user, Battler target)
    {
        yield return View.PlayNormalAttack(action, user, target);
    }

    public IEnumerator PlaySpell(Spell spell, Action action, Battler user, Battler target)
    {
        yield return View.PlaySpell(spell, action, user, target); // Presenter.PlayNormalAttack(action, user, target);
    }

    public IEnumerator FadeInCommands()
    {
        yield return View.ShowCommands();
        View.EnableCommands(true);
    }
    
    public IEnumerator SetBattleResult(string text)
    {
        yield return View.SetBattleResult(text);
    }

    public IEnumerator FadeInBattlers()
    {
        yield return View.FadeInBattlers();
    }

    public IEnumerator FadeOutBattlers()
    {
        yield return View.FadeOutBattlers();
    }

    public IEnumerator Won()
    {
        yield return View.Won();
    }

    public IEnumerator Lost()
    {
        yield return View.Lost();
    }

    public IEnumerator Shake(float multiplier = 1)
    {
        yield return View.Shake(multiplier);
    }

    public IEnumerator AnimateBattleResult(List<Loot> lootTable = null, LevelModel level = null)
    {
        yield return View.AnimateBattleResult(lootTable, level);
    }

    public IEnumerator FadeInLevelUp(CharacterSheet.Attribute[] attributes = null)
    {
        yield return View.FadeInLevelUp(attributes);
    }
    #endregion

    public void EndBattle()
    {
        SetEndButtonVisibility(false);
        View.Hide();
    }

    public bool IsInventoryVisible()
    {
        return View.IsInventoryVisible();
    }

    public bool IsSpellsVisible()
    {
        return View.IsSpellsVisible();
    }
}
