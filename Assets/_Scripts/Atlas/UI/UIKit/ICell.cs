using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Atlas.UIKit
{
    public interface ICell<T>
    {
        public int Id { get; set; }
        public T Data { get; set; }
        public Button Button { get; }

        public void Initialize(int id);
        public void Apply(T data);
    }
}
