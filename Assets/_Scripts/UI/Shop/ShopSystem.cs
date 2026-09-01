using DB;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using System.Linq;
using Atlas.Effects;
using Atlas.Player;
using Atlas.DB;

public class ShopSystem : MonoBehaviour
{
    [SerializeField]
    private bool onlyBuying;

    private bool _opened;

    [Inject]
    private ShopPresenter Presenter { get; set; }
    [Inject]
    public PlayerSystem Player { get; set; }
    [Inject]
    public EffectsSystem Effects { get; set; }

    public ShopModel ShopModel { get; set; }

    public UnityEvent<int> ShopOpened;
    public UnityEvent ShopClosed;

    public UnityEvent<(string, int)> ItemBought;

    private void Awake() 
    {
        Presenter.System = this;
        ShopModel = new ShopModel();

        ShopOpened = new UnityEvent<int>();
        ShopClosed = new UnityEvent();
    }

    public void Open(int shopId)
    {
        Shop shop = Database.Instance.GetShop("first");
        ShopModel.Load(shop);
        Presenter.Open(ShopModel.SellEntries, ShopModel.BuyEntries);

        _opened = true;
        ShopOpened.Invoke(shopId);
    }

    public void Close()
    {
        _opened = false;
        Presenter.Close();
        ShopClosed.Invoke();
    }

    public void Toggle()
    {
        if(!_opened)
            Open(0);
        else
            Close();
    }

    public void Buy(int cellId)
    {
        ShopItemEntry entry = ShopModel.BuyEntries.First(x => x.Id == cellId);

        ItemBought.Invoke((entry.ItemId, entry.Cost));
        Presenter.Update();
    }

    public void Sell(int cellId)
    {
        ShopItemEntry entry = ShopModel.SellEntries[cellId];

        Player.InventorySystem.AddCurrency(entry.Value);
        Player.InventorySystem.TakeOneItem(entry.Id);   

        Debug.Log("Cell Id: " + entry.Id);

        ShopModel.UpdateInventory(Player.InventorySystem);
        Presenter.Update();
    }
}
