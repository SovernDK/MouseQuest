using CharacterSheet;
using System.Collections.Generic;
using Atlas.UI;
using UnityEngine;
using Zenject;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Atlas.DB;
using Attribute = Atlas.DB.Attribute;
using Atlas.Enums;
using System.Linq;

namespace Atlas.Views
{
    public class AttributeView : MonoBehaviour, IView
    {
        [SerializeField]
        public Transform _content; 
        [SerializeField]
        public Transform _attributesContent;
        [SerializeField]
        public Transform _resistancesContent;
        [SerializeField]
        private GameObject _attributeRowPrefab;
        [SerializeField]
        private GameObject _resistanceRowPrefab;

        [SerializeField] [FoldoutGroup("Level")]
        private TMP_Text _level;
        [SerializeField] [FoldoutGroup("Level")]
        private TMP_Text _currentExp;
        [SerializeField] [FoldoutGroup("Level")]
        private TMP_Text _nextExp;
        [SerializeField] [FoldoutGroup("Level")]
        private Slider _levelSlider;

        private List<AttributeRow> _attributeRows;
        private List<ResistanceRow> _resistanceRows;

        [Inject]
        private AttributeRow.Factory _attributeCellFactory;

        [Inject]
        private ResistanceRow.Factory _resistanceCellFactory;

        [Inject]
        private AttributePresenter _presenter;

        public string ViewName => "Attributes";

        public bool Visible => _content.gameObject.activeSelf;

        private void Awake() 
        {
            _presenter.View = this;
            _attributeRows = new List<AttributeRow>();
            _resistanceRows = new List<ResistanceRow>();
        }

        public void Initialize()
        {
        
        }

        public void ApplyAttributes(CharacterSheet.Attribute[] attributes)
        {
            for(int i = 0; i < attributes.Length; i++)
            {
                Attribute attr = Database.Instance.GetAttribute(i);            
                if(i < _attributeRows.Count)
                {
                    _attributeRows[i].Initialize(i);
                    _attributeRows[i].ApplyAttribute(attributes[i], attr.shortcut, attr.description);
                }
                else
                {
                    AttributeRow cellClone = _attributeCellFactory.Create(_attributeRowPrefab);
                    cellClone.transform.SetParent(_attributesContent);
                    cellClone.GetComponent<RectTransform>().localScale = Vector3.one;

                    _attributeRows.Add(cellClone);
                    _attributeRows[i].ApplyAttribute(attributes[i], attr.shortcut, attr.description);
                }

                if(attributes[i].VariableAttribute)
                {
                    _attributeRows[i].gameObject.SetActive(false);
                }
            }
        }

        public void ApplyResistances(CharacterSheet.Resistance[] resistances)
        {   
            List<CharacterSheet.Resistance> currentResistances = resistances.ToList();
            currentResistances.RemoveAt((int) EElement.None);
            
            for(int i = 0; i < currentResistances.Count; i++)
            {
                DB.Resistance res = Database.Instance.GetResistance(i);   

                if(i < _resistanceRows.Count)
                {
                    _resistanceRows[i].Initialize(i);
                    _resistanceRows[i].ApplyResistance(currentResistances[i], res.icon, res.name, res.description);
                }
                else
                {
                    ResistanceRow cellClone = _resistanceCellFactory.Create(_resistanceRowPrefab);
                    cellClone.transform.SetParent(_resistancesContent);
                    cellClone.GetComponent<RectTransform>().localScale = Vector3.one;

                    _resistanceRows.Add(cellClone);
                    _resistanceRows[i].ApplyResistance(currentResistances[i], res.icon, res.name, res.description);
                }
            }
        }

        public void ApplyLevel(LevelModel level)
        {
            // _level.text = LocalizationManager.GetTranslation("ui_level") + " " + level.CurrentLevel.ToString();
            // _currentExp.text = LocalizationManager.GetTranslation("ui_level_curr") + " " + level.CurrentExp.ToString();
            // _nextExp.text = LocalizationManager.GetTranslation("ui_level_next") + " " + level.NextLevelExp.ToString();
            // _levelSlider.maxValue = level.NextLevelExp;
            // _levelSlider.value = level.CurrentExp / level.NextLevelExp;
        }

        public void Hide()
        {
            _content.gameObject.SetActive(false);
        }

        public void Show()
        {
            _content.gameObject.SetActive(true);
        }
    }
}