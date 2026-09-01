using System;
using System.Collections.Generic;
using Atlas.Utility;
using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atlas.DB
{
    [Serializable]
    public class BattlerState
    {
        protected const string SPLIT = "Split";
        protected const string SPLIT_LEFT = "Split/Left";
        protected const string SPLIT_RIGHT = "Split/Right";
        protected const string SPLIT_LEFT_GENERAL = "Split/Left/General";
        protected const string SPLIT_LEFT_PROPERTIES = "Split/Left/Properties";
        protected const string SPLIT_LEFT_ACTIONS = "Split/Left/Actions";
        protected const string SPLIT_LEFT_GENERAL_SPLIT_RIGHT = "Split/Left/General/Split/Right";

        [HorizontalGroup(SPLIT)]
        [VerticalGroup(SPLIT_LEFT)]
        [BoxGroup(SPLIT_LEFT_GENERAL)]
        [HorizontalGroup(SPLIT_LEFT_GENERAL + "/Split", 55, LabelWidth = 70)]
        [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
        public Sprite icon;
        
        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)]
        [ValueDropdown("GetTerms")]
        public string name;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)]
        [ValueDropdown("GetTerms")]
        public string description;

        [VerticalGroup(SPLIT_LEFT_GENERAL_SPLIT_RIGHT)]
        [InfoBox("When applied states with same channel are overwritten (there can only be one state with given channel), set to -1 to ignore this rule")]
        public int channel = -1;
        
        [BoxGroup(SPLIT_LEFT_PROPERTIES)]
        [LabelWidth(80)]
        public EBattlerStateFrequency frequency;

        [BoxGroup(SPLIT_LEFT_PROPERTIES)]
        [LabelWidth(80)]
        public EBattlerStateActivation activation;

        [HorizontalGroup(SPLIT_LEFT_PROPERTIES+"/TurnCount", LabelWidth = 80, Width = 285)]
        public bool timeLimited;

        [HorizontalGroup(SPLIT_LEFT_PROPERTIES+"/TurnCount", LabelWidth = 110)]
        [ShowIf("@timeLimited == true")]
        public int turnsToExpire;

        [BoxGroup(SPLIT_LEFT_ACTIONS)]
        [ValidateInput("ValidateActions", "Target of state action can only be User")]
        public List<ActionType> actions;

        [VerticalGroup(SPLIT_RIGHT)]
        [BoxGroup(SPLIT_RIGHT + "/notes", LabelText = "Notes")]
        [HideLabel, TextArea(4, 9)]
        public string notes;

        private List<string> GetTerms()
        {
            return LocalizationManager.GetTermsList();
        }

        private bool ValidateActions()
        {
            if(actions == null || actions.Count <= 0) return true;

            foreach(ActionType action in actions)
            {
                if(action.target == Enums.ETarget.Both || action.target == Enums.ETarget.Other) 
                {
                    action.target = Enums.ETarget.User;
                    return false;
                }
            }
            
            return true;
        }

        public void Create(string newName)
        {
            name = Util.ToItemSnakeCase(newName, "bstate_");
            description = Util.ToItemSnakeCase(newName, "bstate_", "_description");
        }
    }

    public enum EBattlerStateFrequency
    {
        Once, EveryTurn
    }

    public enum EBattlerStateActivation
    {
        OnExecution, TurnEnd
    }
}