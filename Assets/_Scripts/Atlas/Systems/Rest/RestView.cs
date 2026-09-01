using Atlas.Presenters;
using Atlas.UI;
using UnityEngine;
using Zenject;

namespace Atlas.Views
{
    public class RestView : MonoBehaviour, IView
    {
        [SerializeField]
        private Transform _content;

        [Inject]
        private RestPresenter _restPresenter;

        public string ViewName => "RestView";

        public bool Visible => _content.gameObject.activeSelf;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            _restPresenter.View = this;
        }

        public void Enter()
        {
            Show();
        }

        public void End()
        {
            Hide();
        }

        public void Show()
        {
            _content.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _content.gameObject.SetActive(false);
        }
    }
}