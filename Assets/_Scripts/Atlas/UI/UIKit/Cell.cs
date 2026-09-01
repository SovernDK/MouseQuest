using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Atlas.UIKit
{
    public class Cell<T> : MonoBehaviour, ICell<T>
    {
        [HorizontalGroup("Split", Width = 0.3f)] 
        [LabelWidth(70)]
        public bool interactable;

        [HorizontalGroup("Split",Width = 0.7f)]
        [ShowIf("@interactable == true")]
        [HideLabel]
        [SerializeField] 
        private Button _button;

        public int Id { get; set; }
        public T Data { get; set; }
        public Button Button { get => _button; }

        public virtual void Apply(T data)
        {
            Data = data;
        }

        public virtual void Initialize(int id)
        {
            Id = id;   
        }

        public class Factory : PlaceholderFactory<Object, T> {}
    }
}
