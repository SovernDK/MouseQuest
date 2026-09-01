using System.Collections;

namespace Atlas.Core 
{
    public interface IStateAdvanced<T1>
    {
        public T1 Context { get; set; }
        public int StateId { get; set; }
        public IEnumerator OnEnter();
        public IEnumerator OnUpdate();
        public IEnumerator OnExit();
    }
}