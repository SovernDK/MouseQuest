using System.Collections;
using Combat;
using UnityEngine;
using Sirenix.OdinInspector;
using Atlas.AI;
using Atlas.DB;
using Atlas.Battle;
using Atlas.Enums;
using CharacterSheet;
using Atlas.Utility;

public class EnemyBattler : Battler
{
    private EnemyMoveDecider _enemyMoveDecider;

    public override EBattleAnchorSide Side => EBattleAnchorSide.Left;

    public EnemyMoveDecider EnemyMoveDecider { get => _enemyMoveDecider; set => _enemyMoveDecider = value; }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override IEnumerator MakeMove()
    {
        string decidedQuick = _enemyMoveDecider.DecideQuickMove();
        if(!decidedQuick.Equals("none"))
            _battleSystem.SpellCommand(decidedQuick);

        var decided = _enemyMoveDecider.DecideNormalMove();
        switch(decided.Item1)
        {
            case EEnemyMove.Attack:
                _battleSystem.AttackCommand();
                break;
            case EEnemyMove.Spellcast:
                _battleSystem.SpellCommand(decided.Item2);
                break;
            case EEnemyMove.Transformation:
                _battleSystem.TransformationCommand(Database.Instance.GetEnemy(decided.Item3), true);
                break;
        }
        
        yield return null;
    }

    public Battler CreateBattlerFromEnemy(Enemy enemy)
    {
        _name = enemy.name;
        _battlerSprite.sprite = enemy.icon;
        _battlerSprite.transform.localScale = enemy.battlerScale;
        BasicAttackEffect = enemy.attackEffect.name;
        Busy = false;
        
        _enemyMoveDecider = new EnemyMoveDecider(this);
        _enemyMoveDecider.Set(enemy);

        BaseAtkFormula = enemy.formula;
        
        Attributes = new AttributesModel();
        Attributes.BaseElement = enemy.attackElement;

        foreach(AttributeValue av in enemy.startingAttributeValues)
            Attributes.SetAttribute(av.attribute, av.value);

        foreach(ResistanceValue res in enemy.startingResistanceValues)
            Attributes.SetResistance(res.element, res.value);

        Attributes.SetAttributeValue(EAttribute.Hitpoints, Attributes.GetMaxValue(EAttribute.Hitpoints));
        
        return this;
    }

    public override DamageAction GetAttackAction()
    {
        Debug.Log($"Element {BaseAtkElement}");
        return new DamageAction(ETarget.Other, BaseAtkFormula, BaseAtkElement);
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
}
