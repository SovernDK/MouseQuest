using System.Collections;

namespace Atlas.Core 
{
    public interface IState<T1>
    {
        public T1 Context { get; set; }
        public int StateId { get; set; }
        public void OnEnter();
        public IEnumerator OnUpdate();
        public void OnExit();
    }
}