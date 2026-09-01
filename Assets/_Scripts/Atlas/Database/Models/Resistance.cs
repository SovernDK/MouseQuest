using System;
using System.Collections.Generic;
using Atlas.Enums;
using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [Serializable]
    public class Resistance
    {
        [HorizontalGroup("Split")]
        [HorizontalGroup("Split/Left")]
        public EElement id;
        
        [HorizontalGroup("Split/Left")]
        [PreviewField]
        public Sprite icon;

        [VerticalGroup("Split/Right")]
        [BoxGroup("Split/Right/Translations")]
        [ValueDropdown("GetTerms")] 
        public string name;

        [BoxGroup("Split/Right/Translations")]
        [ValueDropdown("GetTerms")]
        public string description;

        private List<string> GetTerms()
        {
            return LocalizationManager.GetTermsList();
        }
    }
}