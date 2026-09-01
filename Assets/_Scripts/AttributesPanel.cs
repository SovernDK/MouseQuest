using System.Collections.Generic;
using Atlas.DB;
using Atlas.Enums;
using CharacterSheet;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace Atlas.UI
{
    public class AttributesPanel : MonoBehaviour
    {
        public GameObject prefab;
        public Transform parent;

        private List<GameObject> _attributes;

        private void Awake() 
        {
            _attributes = new List<GameObject>();
        }

        public void UpdateAttributes(AttributesModel model)
        {
            if(_attributes == null) 
                _attributes = new List<GameObject>();

            _attributes.ForEach(a => a.SetActive(false)); 

            for(int i = 0; i < model.Attributes.Length; i++)
            {
                DB.Attribute atr = Database.Instance.GetAttribute(i);
                if(i < _attributes.Count)
                {
                    string shortcut = LocalizationManager.GetTranslation(atr.shortcut);
                    int value = (i == (int) EAttribute.Hitpoints) ? model.GetMaxValue(i) : model.GetValue(i);

                    _attributes[i].GetComponentInChildren<TMP_Text>().text = $"{shortcut}\n{value}";
                    _attributes[i].gameObject.SetActive(true);
                }
                else
                {
                    GameObject clone = Instantiate(prefab, parent);
                    string shortcut = LocalizationManager.GetTranslation(Database.Instance.GetAttribute(i).shortcut);
                    int value = (i == (int) EAttribute.Hitpoints) ? model.GetMaxValue(i) : model.GetValue(i);

                    clone.GetComponentInChildren<TMP_Text>().text = $"{shortcut}\n{value}";
                    
                    _attributes.Add(clone);
                }
            }
        }
    }
}