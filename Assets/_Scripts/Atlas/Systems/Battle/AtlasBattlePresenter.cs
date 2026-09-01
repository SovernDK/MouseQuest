using System.Collections;
using Atlas.Systems;
using Atlas.UI;
using Atlas.Views;

namespace Atlas.Presenters
{
    public class AtlasBattlePresenter : IPresenter<AtlasBattleSystem, AtlasBattleView>
    {
        public AtlasBattleSystem System { get; set; }
        public AtlasBattleView View { get; set; }

        public void StartBattle()
        {
            View.StartBattle();
        }

        public void StartTurn()
        {
            View.StartTurn();
        }

        public void PlayerTurn()
        {
            View.PlayerTurn();
        }

        public void EnemiesTurn()
        {
            View.EnemiesTurn();
        }

        public void EndBattle()
        {
            View.EndBattle();
        }

        public IEnumerator ShowNotification(string message)
        {
            yield return View.ShowNotification(message);
        }

        public void EnableCommands(bool enabled)
        {
            View.EnableCommands(enabled);
        }

        public void Hide()
        {
            View.Hide();
        }
    }
}
