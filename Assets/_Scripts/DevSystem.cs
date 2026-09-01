using Atlas.DB;
using Atlas.Player;

// using DB;
// ;
using UnityEngine;
using Zenject;

public class DevSystem : MonoBehaviour
{
    [Inject]
    private DevPresenter _presenter;
    [Inject]
    private DatabaseSystem _database;
    [Inject]
    public PlayerSystem Player { get; set; }

    public InventorySystem InventorySystem { get; set; }

    private void Awake() 
    {
        _presenter.System = this;
        InventorySystem = FindFirstObjectByType<InventorySystem>();
    }

    private void Start()
    {
        _presenter.Initialize();
    }

    private void Update() 
    {
        if(Input.GetKeyUp(KeyCode.F1))
        {
            if(_presenter.IsViewVisible()) Hide();
            else Show();
        }    
    }    

    public void AddCurrency()
    {
        _presenter.AddCurrency();
    }

    public void AddExp()
    {
        _presenter.AddExp();
    }

    public void SetHP()
    {
        _presenter.SetHP();
    }

    public void Show()
    {
        _presenter.Show();
    }

    public void Hide()
    {
        _presenter.Hide();
    }

    public Item[] GetAllItems()
    {
        return _database.GetItems().ToArray();
    }
}
