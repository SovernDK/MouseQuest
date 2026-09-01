using System.Collections;
using Atlas.Presenters;
using Atlas.UI;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Atlas.Views
{
    public class AtlasBattleView : MonoBehaviour, IView
    {
        [SerializeField]
        private Transform _content;
        [SerializeField]
        private GameObject _centerText;
        [SerializeField]
        private Transform _commandButtons;

        [Inject]
        private AtlasBattlePresenter _presenter;

        public string ViewName => "Battle";
        public bool Visible => _content.gameObject.activeSelf;

        private void Awake() 
        {
            Initialize();
        }
        
        #region IView 
        public void Initialize()
        {
            _presenter.View = this;
        }

        public void Show()
        {
            _content.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _content.gameObject.SetActive(false);
        }
        #endregion

        public void StartBattle()
        {
            EnableCommands(false);
        }

        public void StartTurn()
        {
            DrawCenterText("Turn Start");
        }

        public void PlayerTurn()
        {
            DrawCenterText("Player Turn");
        }

        public void EnemiesTurn()
        {
            EnableCommands(false);
            DrawCenterText("Enemies Turn");
        }

        public void EndBattle()
        {
            DrawCenterText("END");
        }

        public IEnumerator ShowNotification(string message)
        {
            DrawCenterText(message);
            yield return new WaitForSeconds(1f);
        }

        public void EnableCommands(bool enabled)
        {
            foreach(Transform child in _commandButtons.transform)
            {
                child.GetComponent<Button>().interactable = enabled;
            }
        }

        #region privates
        private void DrawCenterText(string text)
        {
            _centerText.GetComponentInChildren<TMP_Text>().text = text;
            _centerText.GetComponent<MMF_Player>().PlayFeedbacks();
        }
        #endregion
    }
}
