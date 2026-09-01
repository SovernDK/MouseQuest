using Atlas.DB;
using Atlas.Systems;
using Zenject;

namespace Atlas.UIKit 
{
    public class SpellRewardPanel : AtlasList<SpellCardCell, Spell>
    {
        [Inject]
        AtlasBattleSystem _battleSystem;

        protected override void OnButtonClicked(int id)
        {
            base.OnButtonClicked(id);
            _battleSystem.ChooseSpellReward(Cells[id].Data.name);
        }
    }
}
