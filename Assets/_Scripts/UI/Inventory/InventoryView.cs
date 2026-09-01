using System.Collections.Generic;
using Atlas.Core;
using Atlas.DB;
using Atlas.UI;
using DB;
using ModestTree;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

public class InventoryView : MonoBehaviour, IView
{
    [SerializeField]
    private Transform _content;
    
    [SerializeField]
    private Transform _itemsContent;

    [SerializeField] [FoldoutGroup("Equipment")]
    private Transform _equipmentContent;

    [SerializeField]
    private GameObject _inventoryCellPrefab;

    [SerializeField] [FoldoutGroup("Equipment")]
    private GameObject _equipmentCellPrefab;

    [SerializeField]
    private int _inventorySize;
    
    [SerializeField]
    private TMP_Text _currencyValue;

    [SerializeField]
    private bool _showEmptySlots;

    [Inject]
    private InventoryPresenter _presenter;
    
    [Inject]
    private InventoryCell.Factory _inventoryCellFactory;
    private List<InventoryCell> _inventoryCells;

    [SerializeField] [FoldoutGroup("Equipment")]
    private List<EquipmentCell> _equipmentRows;

    #region IView
    public string ViewName => "Inventory";
    public bool Visible => _content.gameObject.activeSelf;
    #endregion

    public List<InventoryCell> Cells { get => _inventoryCells; set => _inventoryCells = value; }
    public int InventorySize { get => _inventorySize; set => _inventorySize = value; }

    private void Awake() 
    {
        Initialize();
    }

    public void Initialize()
    {
        _presenter.View = this;
        _inventoryCells = new List<InventoryCell>();
    }

    #region IView
    public void ApplyInventory(InventorySlot[] slots)
    {
        for(int i = 0; i < _inventorySize; i++)
        {
            if(i < _inventoryCells.Count)
            {
                int cellId = slots[i].Id;

                _inventoryCells[i].ApplyItem(Database.Instance.GetItem(slots[i].ItemId), slots[i].Amount);
                _inventoryCells[i].Connect(() => { _presenter.System.UseItem(cellId); });
            }
            else
            {
                string itemId = slots[i].ItemId;
                int cellId = slots[i].Id;

                InventoryCell cellClone = _inventoryCellFactory.Create(_inventoryCellPrefab);
                cellClone.transform.SetParent(_itemsContent);

                _inventoryCells.Add(cellClone);
                _inventoryCells[i].Initialize(cellId);
                _inventoryCells[i].ApplyItem(Database.Instance.GetItem(itemId), slots[i].Amount);
                _inventoryCells[i].Connect(() => { _presenter.System.UseItem(cellId); });
            }

            if(slots[i].IsEmpty() && !_showEmptySlots) 
                _inventoryCells[i].GetComponent<ButtonAdditional>().Deactivate();
            else 
                _inventoryCells[i].gameObject.SetActive(true);
        }
    }

    public void ApplyCurrency(int value)
    {
        // _currencyValue.text = value.ToString();
    }

    public void ApplyEquipment(EquipmentSlot[] slots)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < _equipmentRows.Count)
            {
                _equipmentRows[i].ApplyItem(Database.Instance.GetItem(slots[i].ItemId));
            }
        }
    }

    public void InitializeEquipmentSlots()
    {
        int i = 0;
        foreach(EquipmentCell cell in _equipmentRows)
        {
            cell.Initialize(i, Database.Instance.GetItem("item_empty"));
            cell.OnClicked.AddListener(_presenter.System.Unequip);
            
            i++;
        }
    }

    public void EnableInventory(bool enable)
    {
        for(int i = 0; i < _inventoryCells.Count; i++)
        {
            _inventoryCells[i].Button.interactable = enable;
        }
    }

    public void Hide()
    {
        _content.gameObject.SetActive(false);
    }

    public void Show()
    {
        _content.gameObject.SetActive(true);
    }
    #endregion
}   
