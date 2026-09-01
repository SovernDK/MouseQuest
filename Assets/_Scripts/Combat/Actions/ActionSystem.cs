using Atlas.Core;
using Combat;
using DB;
using UnityEngine;
using Zenject;

public class ActionSystem : MonoBehaviour, ISystem
{
    public void Initialize()
    {
        
    }

    /*************************
    *** Target = args[0]
    *** Value = args[1]
    *** ElementId = args[2]
    *** AttributeId = args[3]
    *** IsTimeLimited = args[4]
    *** TurnCount = args[5]
    *************************/
    public Combat.Action GetAction(int id, int[] args)
    {
        // switch(id)
        // {
        //     case (int) EActionType.Heal:
        //         return new HealAction((ETarget) args[0], args[1]);
        //     case (int) EActionType.Damage:
        //         return new DamageAction((ETarget) args[0], args[1], args[2]);
        //     case (int) EActionType.MagicDamage:
        //         return new MagicDamageAction((ETarget) args[0], args[1], args[2]);
        //     case (int) EActionType.AttributeIncrease:
        //         return new AttributeIncreaseAction((ETarget) args[0], args[1], args[3], args[4], args[5]);
        //     case (int) EActionType.AttributeDecrease:
        //         return new AttributeDeacreaseAction((ETarget) args[0], args[1], args[3]);
        //     default:
        //         return new HealAction((ETarget) args[0], args[1]);
        // }
        return null;
    }

    public EBattlerAnimation GetAnimation(int id)
    {
        return 0;
        // switch(id)
        // {
        //     case (int) EActionType.Heal:
        //         return EBattlerAnimation.Heal;
        //     case (int) EActionType.Damage:
        //         return EBattlerAnimation.GetHit;
        //     case (int) EActionType.MagicDamage:
        //         return EBattlerAnimation.GetHit;
        //     case (int) EActionType.AttributeIncrease:
        //         return EBattlerAnimation.Buff;
        //     case (int) EActionType.AttributeDecrease:
        //         return EBattlerAnimation.Debuff;
        //     default:
        //         return EBattlerAnimation.Heal;
        // }
    }
}