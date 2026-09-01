using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.Battle
{
    public class BattleEffectsAnchor : MonoBehaviour
    {
        [Serializable]
        private class Anchor
        {
            [HideLabel] [HorizontalGroup("Anchors")]
            public EBattleAnchor id;
            [HideLabel] [HorizontalGroup("Anchors")]
            public Transform anchor;
        }

        [SerializeField]
        private List<Anchor> _leftAnchors;
        [SerializeField]
        private List<Anchor> _midAnchors;
        [SerializeField]
        private List<Anchor> _rightAnchors;

        private List<Anchor> LeftAnchors { get => _leftAnchors; set => _leftAnchors = value; }
        private List<Anchor> MidAnchors { get => _midAnchors; set => _midAnchors = value; }
        private List<Anchor> RightAnchors { get => _rightAnchors; set => _rightAnchors = value; }

        public Transform Get(EBattleAnchorSide side, EBattleAnchor anchor)
        {
            switch(side)
            {
                case EBattleAnchorSide.Left:
                    return _leftAnchors.Find(a => a.id == anchor).anchor;
                case EBattleAnchorSide.Center:
                    return _midAnchors.Find(a => a.id == anchor).anchor;
                case EBattleAnchorSide.Right:
                    return _rightAnchors.Find(a => a.id == anchor).anchor;
                default:
                    return _leftAnchors.Find(a => a.id == anchor).anchor;
            }
        }
    }

    public enum EBattleAnchor
    {
        Top, Bottom, Middle
    }

    public enum EBattleAnchorSide
    {
        Center, Left, Right
    }
}