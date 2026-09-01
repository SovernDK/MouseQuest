using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using Zenject;

namespace Atlas.UIKit
{
    public class AtlasList<Cell, Data> : MonoBehaviour where Cell : MonoBehaviour, ICell<Data>
    {
        [SerializeField]
        private Transform _parentContent;

        [SerializeField]
        private Transform _content;

        [SerializeField]
        private Transform _prefab;
        private List<Cell> _cells;

        [Inject]
        private DiContainer _container;
        private bool _initialized;

        public List<Cell> Cells { get => _cells; set => _cells = value; }

        protected virtual void Initialize()
        {
            _cells = new List<Cell>();
            _initialized = true;
        }

        public virtual void List(List<Data> data)
        {
            if(!_initialized) Initialize();

            for(int i = 0; i < data.Count; i++)
            {
                if(i < _cells.Count)
                {
                    _cells[i].Apply(data[i]);

                    if(_cells[i].Button != null) 
                    {
                        int id = _cells[i].Id;
                        _cells[i].Button.onClick.AddListener(() => { OnButtonClicked(id); });
                    }

                    if(_cells[i].TryGetComponent(out MMF_Player player))
                        player.PlayFeedbacks();
                }
                else
                {
                    Cell cellClone = _container.InstantiatePrefabForComponent<Cell>(_prefab, _content);

                    _cells.Add(cellClone);
                    _cells[i].Initialize(i);
                    _cells[i].Apply(data[i]);

                    if(_cells[i].Button != null) 
                    {
                        int id = _cells[i].Id;
                        _cells[i].Button.onClick.AddListener(() => { OnButtonClicked(id); });
                    }

                    if(_cells[i].TryGetComponent(out MMF_Player player))
                        player.PlayFeedbacks();
                }
            }
        }

        protected void Play()
        {
            for(int i = 0; i < _cells.Count; i++)
            {
                if(_cells[i].TryGetComponent(out MMF_Player player)) player.PlayFeedbacks();
            }
        }

        protected virtual void OnButtonClicked(int id)
        {
            // Debug.Log($"Spell ID {id} clicked");
        }

        public void Show()
        {
            _parentContent.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _parentContent.gameObject.SetActive(false);
        }
    }
}
