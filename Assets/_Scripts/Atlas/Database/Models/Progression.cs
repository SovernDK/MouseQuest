using System;
using UnityEngine;

namespace Atlas.DB
{
    [Serializable]
    public class Progression
    {
        [SerializeField]
        public AnimationCurve _exp;
        [SerializeField]
        public AnimationCurve _hp;
        [SerializeField]
        public AnimationCurve _atk;
        [SerializeField]
        public AnimationCurve _def;
        [SerializeField]
        public AnimationCurve _matk;
        [SerializeField]
        public AnimationCurve _mdef;
        [SerializeField]
        public AnimationCurve _spd;
    }
}