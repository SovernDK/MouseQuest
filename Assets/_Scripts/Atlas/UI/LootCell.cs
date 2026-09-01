using Atlas.DB;
using Atlas.UIKit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Atlas.UI
{
    public class LootCell : Cell<Loot>
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMP_Text _label;
        [SerializeField]
        private TMP_Text _amount;

        public override void Apply(Loot data)
        {
            base.Apply(data);

            _icon.sprite = data.prototype.item.icon;
            _label.text = $"{data.prototype.item.name}";
            _amount.text = $"{data.amount}";
        }
    }
}
