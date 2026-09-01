using System.Collections.Generic;
using Atlas.Core;
using Atlas.DB;
using Atlas.UI;
using DB;
using UnityEngine;
using Zenject;

public class SpellView : MonoBehaviour, IView
{
    
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Transform _spellContent;
    [SerializeField]
    private GameObject _spellCellPrefab;
    private List<SpellCell> _spellCells;

    [Inject]
    public SpellPresenter Presenter{ get; set; }
    [Inject]
    public ResourcesSystem Resources { get; set; }


    public string ViewName => "Spells";

    public bool Visible => _content.gameObject.activeSelf;

    private void Awake() 
    {
        Presenter.View = this;

        _spellCells = new List<SpellCell>();
    }

    public void Hide()
    {
        _content.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        
    }

    public void Show()
    {
        _content.gameObject.SetActive(true);
    }

    public void ApplySpells(SpellEntry[] entries)
    {
        for(int i = 0; i < entries.Length; i++)
        {
            if(i < _spellCells.Count)
            {
                _spellCells[i].ApplySpell(entries[i]);
            }
            else
            {
                GameObject cellClone = Instantiate(_spellCellPrefab, _spellContent);
                _spellCells.Add(cellClone.GetComponent<SpellCell>());
                _spellCells[i].Initialize(i);
                _spellCells[i].ApplySpell(entries[i]);
                _spellCells[i].OnClicked.AddListener(Presenter.CastSpell);
            }      
        }
    }

    public void EnableSpells(bool enable)
    {
        for(int i = 0; i < _spellCells.Count; i++)
        {
            _spellCells[i].Button.interactable = enable;
        }
    }
}