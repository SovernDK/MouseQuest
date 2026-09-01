using Atlas.DB;
using Atlas.Utility;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Atlas.UIKit 
{
    public class SpellCardCell : Cell<Spell>
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _label;

        [SerializeField]
        private Image _typeIcon;
        
        [SerializeField]
        private TMP_Text _type;

        public override void Apply(Spell data)
        {
            base.Apply(data);

            _icon.sprite = data.icon;

            if(LocalizationManager.TryGetTranslation(data.name, out string translation))
                _label.text = translation;
            else 
                _label.text = data.name;

            _typeIcon.sprite = (data.castType == ECastType.Quick) ? Config.Instance.castQuick : Config.Instance.castNormal;
            _type.text = $"{data.castType}";
        }
    }
}
