using System;
using System.Collections.Generic;
using Atlas.Enums;
using I2.Loc;
using Sirenix.OdinInspector;

namespace Atlas.DB
{
    [Serializable]
    public class Attribute
    {
        [HorizontalGroup("Split")]
        [HorizontalGroup("Split/Left")]
        [HideLabel]
        public EAttribute id;
        
        [VerticalGroup("Split/Right")]
        [BoxGroup("Split/Right/Translations")]
        [ValueDropdown("GetTerms")] 
        public string name;
        
        [BoxGroup("Split/Right/Translations")]
        [ValueDropdown("GetTerms")] 
        public string shortcut;

        [BoxGroup("Split/Right/Translations")]
        [ValueDropdown("GetTerms")]
        public string description;

        private List<string> GetTerms()
        {
            return LocalizationManager.GetTermsList();
        }
    }
}