using Atlas.DB;
using Atlas.Enums;
using Atlas.Player;
using CharacterSheet;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class AttributeSystem : MonoBehaviour
{
    private LevelModel _level;
    private AttributesModel _attributes;
  
    [Inject]
    public AttributePresenter Presenter { get; }
    
    [Inject]
    public PlayerSystem Player { get; }

    public AttributesModel Attributes { get => _attributes; set => _attributes = value; }
    public LevelModel Level { get => _level; set => _level = value; }

    public UnityEvent<int> AttributeValueChanged;
    public UnityEvent<AttributesModel> AttributesRefreshed;

    [BoxGroup("Debug")]
    public bool debug;

    private void Awake()
    {
        Presenter.System = this;

        _attributes = new AttributesModel();
        _level = new LevelModel(0, 0, 10);
    }

    private void Start() 
    {
        PlayerCharacter pc = Player.PlayerCharacter;
        
        //Attributes
        foreach(AttributeValue av in pc.startingAttributeValues)
        {
            Debug.Log($"ATTRIBUTES: {av.attribute} {av.value}");
            _attributes.SetAttribute(av.attribute, av.value);
        }

        //Resistances
        foreach(ResistanceValue res in pc.startingResistanceValues)
        {
            _attributes.SetResistance(res.element, res.value);
        }

        _attributes.SetAttributeValue(EAttribute.Hitpoints, _attributes.GetMaxValue(EAttribute.Hitpoints));

        Presenter.ApplyAttributes(_attributes.Attributes);
        Presenter.ApplyResistances(_attributes.Resistance);
    }

    public void Refresh()
    {
        AttributesRefreshed.Invoke(_attributes);
        Presenter.ApplyAttributes(_attributes.Attributes);
        Presenter.ApplyResistances(_attributes.Resistance);
        // Presenter.ApplyLevel(_level);    
    }

    public void IncreaseAttributeValue(EAttribute type, int value)
    {
        AttributesRefreshed.Invoke(_attributes);
        _attributes.IncreaseAttribute(type, value);
    }

    public void DecreaseAttributeValue(EAttribute type, int value)
    {
        AttributesRefreshed.Invoke(_attributes);
        _attributes.DecreaseAttribute(type, value);
    }

    public void SetAttributeValue(EAttribute type, int value)
    {
        AttributesRefreshed.Invoke(_attributes);
        _attributes.SetAttributeValue(type, value);
    }

    public int GetValue(EAttribute type)
    {
        return _attributes.GetValue(type);
    }

    public int GetMaxValue(EAttribute type)
    {
        return _attributes.GetMaxValue(type);
    }

    public void IncreaseExp(float value)
    {
        Level.IncreaseExp(value);
    }

    public void LevelUp()
    {
        if(!Level.LevelUp && Level.CurrentLevel < 10) return;

        // (Level, float) newLevel = GetLevelByExp(Level.CurrentExp);
        
        // ApplyLevelAttributes(newLevel.Item1);
        // Level.Setlevel(newLevel.Item1.id, newLevel.Item2);
        // Refresh();

        if(debug) //Add pro console
        {
            Debug.Log("Level Up! Now at lv. " + Level.CurrentLevel);
            Debug.Log("Current exp: " + Level.CurrentExp + " / " + Level.NextLevelExp);
        }
    }

    public void CalculateEquipmentModifiers(string[] equipmentItemIds)
    {
        _attributes.RemoveAllEquipmentModifiers();
        foreach(string itemId in equipmentItemIds)
        {
            ConsoleProDebug.LogToFilter($"Item id: {itemId}", "InventorySystem");
            Item item = Database.Instance.GetItem(itemId);
            foreach(Atlas.DB.AttributeModifier modifier in item.modifiers)
            {
                _attributes.AddModifier(modifier.id, $"equipment_{item.name}", modifier.value, false);
            }
        }
    }

    public void Show()
    {
        Presenter.Show();
    }

    public void Hide ()
    {
        Presenter.Hide();   
    }

    public void Toogle()
    {
        Presenter.Toogle();
    }

    // private (Level, float) GetLevelByExp(float exp)
    // {
    //     // for(int i = 0; i < Depot.LevelProgressions[0].levels.Count; i++)
    //     // {
    //     //     float min = Depot.LevelProgressions[0].levels[i].exp;
    //     //     float max = (i+1 < Depot.LevelProgressions[0].levels.Count) ? Depot.LevelProgressions[0].levels[i+1].exp : 9999;

    //     //     Vector2 range = new Vector2(min, max);

    //     //     if(Level.CurrentExp >= range.x && Level.CurrentExp <= range.y) 
    //     //         return (Depot.LevelProgressions[0].levels[i], max);
    //     // } 

    //     // return (Depot.LevelProgressions[0].levels[0], Depot.LevelProgressions[0].levels[1].exp);
    //     return (new Level(), 1);
    // }
}
