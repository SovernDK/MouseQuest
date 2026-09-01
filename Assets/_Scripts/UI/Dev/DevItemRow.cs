// ;
using Atlas.DB;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DevItemRow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _nameLabel;
    [SerializeField]
    private Button _button;

    public int ItemId { get; set; }
    public UnityEvent<int> OnClicked { get; set; }

    public void Initialize()
    {
        OnClicked = new UnityEvent<int>();    
        _button.onClick.AddListener(() => { OnClicked.Invoke(ItemId); });
    }

    public void ApplyItem(Item item)
    {
        ItemId = item.id;
        _nameLabel.text = item.name;
    }
}
