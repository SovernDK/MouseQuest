using System.Collections.Generic;
using Atlas.DB;
using Atlas.Player;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class InventorySystem : MonoBehaviour
{
    [SerializeField]
    private int _inventorySize;

    private InventoryModel _inventoryModel;
    private EquipmentModel _equipmentModel;

    [Inject]
    private InventoryPresenter Presenter { get; set; }

    [Inject]
    private PlayerSystem Player { get; set; }

    public int InventorySize { get => _inventorySize; set => _inventorySize = value; }
    public InventoryModel InventoryModel { get => _inventoryModel; set => _inventoryModel = value; }
    public EquipmentModel EquipmentModel { get => _equipmentModel; set => _equipmentModel = value; }

    public UnityEvent<string[]> Equiped;
    public UnityEvent<string[]> UnEquiped;
    public UnityEvent<Item> ItemAdded;
    public UnityEvent<Item> ItemConsumed;

    private void Awake() 
    {
        _inventoryModel     = new InventoryModel(_inventorySize);    
        _equipmentModel     = new EquipmentModel();    

        Presenter.System = this;
    }

    private void Start() 
    {
        PlayerCharacter pc = Player.PlayerCharacter;

        foreach(InventoryValue v in pc.startingInventory)
        {
            _inventoryModel.Add(v.item.item.name, v.amount);
        }

        foreach(EquipmentValue equipment in pc.startingEquipment)
        {
            if(equipment.item != null)
            {
                _equipmentModel.Set((int) equipment.slot, equipment.item.item.name);
            }
        }

        InventoryModel.CurrentCurrency = 0;

        Equiped.Invoke(_equipmentModel.GetAll());
        Presenter.InitializeInventory();
    }

    public void Show()
    {
        Presenter.Show();   
    }

    public void Hide()
    {
        Presenter.Hide();   
    }

    public void Toogle()
    {
        Presenter.Toogle();  
    }


    public void Buy((string, int) entry)
    {
        if(entry.Item2 <= _inventoryModel.CurrentCurrency)
        {
            TakeCurrency(entry.Item2);
            AddItem(entry.Item1);
        }
    }

    public InventorySlot[] GetItemsByType(EItemType type)
    {
        List<InventorySlot> slots = new List<InventorySlot>(); 

        foreach(InventorySlot slot in InventoryModel.Slots)
        {
            if(Database.Instance.GetItem(slot.ItemId).itemType == type)
            {
                slots.Add(slot);
            }
        }

        return slots.ToArray();
    }

    public InventorySlot[] GetConsumables()
    {
        List<InventorySlot> slots = new List<InventorySlot>(); 

        foreach(InventorySlot slot in InventoryModel.Slots)
        {
            if(Database.Instance.GetItem(slot.ItemId).consumable == true)
            {
                slots.Add(slot);
            }
        }

        return slots.ToArray();
    }

    public void Refresh()
    {
        Presenter.UpdateInventory(InventoryModel.Slots);
        Presenter.UpdateEquipment(EquipmentModel.Slots);
    }

    public void AddItem(string itemId)
    {
        _inventoryModel.Add(itemId);
        Presenter.UpdateInventory(InventoryModel.Slots);
        ItemAdded.Invoke(Database.Instance.GetItem(itemId));
    }

    public void AddItem(string itemId, int amount)
    {
        _inventoryModel.Add(itemId, amount);
        Presenter.UpdateInventory(InventoryModel.Slots);
        ItemAdded.Invoke(Database.Instance.GetItem(itemId));
    }

    public void AddCurrency(int value)
    {
        InventoryModel.CurrentCurrency = Mathf.Clamp(InventoryModel.CurrentCurrency + value, 0, InventoryModel.MaxCurrency);
        Presenter.UpdateCurrency(InventoryModel.CurrentCurrency);
    }

    public void TakeCurrency(int value)
    {
        InventoryModel.CurrentCurrency = Mathf.Clamp(InventoryModel.CurrentCurrency - value, 0, InventoryModel.MaxCurrency);
        Presenter.UpdateCurrency(InventoryModel.CurrentCurrency);
    }

    public void RemoveItem(int cellId)
    {
        _inventoryModel.Remove(cellId);
        Presenter.UpdateInventory(InventoryModel.Slots);
    }

    public void TakeItem(int cellId, int amount)
    {
        _inventoryModel.Take(cellId, amount);
        Presenter.UpdateInventory(InventoryModel.Slots);
    }

    public void TakeOneItem(int cellId)
    {
        _inventoryModel.Take(cellId, 1);
        Presenter.UpdateInventory(InventoryModel.Slots);
    }

    public void UseItem(int cellId)
    {
        Item item = Database.Instance.GetItem(_inventoryModel.Get(cellId));
        if(!item.name.Equals("item_empty"))
        {   
            if(item.equipable && item.consumable) 
                ConsoleProDebug.LogToFilter($"Item with Id {item.id} is both equipable and consumable!", "InventorySystem");
            
            if(item.equipable)
            {
                Equip((int) item.equipmentSlot, cellId);
            }
            else if(item.consumable)
            {
                ItemConsumed.Invoke(item);
                TakeOneItem(cellId);
            }
        }
    }

    public void EnableInventory(bool enabled)
    {
        Presenter.EnableInventory(enabled);
    }

    public void Equip(int equipmentCellId, int inventoryCellId)
    {
        string itemId = _inventoryModel.Get(inventoryCellId);

        if(!_equipmentModel.Get(equipmentCellId).Equals("item_empty"))
        {
            _inventoryModel.Add(_equipmentModel.Get(equipmentCellId));
        }

        _equipmentModel.Set(equipmentCellId, itemId);
        _inventoryModel.Take(inventoryCellId, 1);

        Equiped.Invoke(_equipmentModel.GetAll());

        Presenter.UpdateInventory(InventoryModel.Slots);
        Presenter.UpdateEquipment(EquipmentModel.Slots);
    }

    public void Unequip(int cellId)
    {
        if(!_equipmentModel.Get(cellId).Equals("item_empty"))
        {
            _inventoryModel.Add(_equipmentModel.Get(cellId));
        }
        _equipmentModel.Remove(cellId);

        UnEquiped.Invoke(_equipmentModel.GetAll());

        Presenter.UpdateInventory(InventoryModel.Slots);
        Presenter.UpdateEquipment(EquipmentModel.Slots);
    }
}
