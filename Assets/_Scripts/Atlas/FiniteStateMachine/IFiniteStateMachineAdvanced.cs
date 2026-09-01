using System.Collections;
using System.Collections.Generic;

namespace Atlas.Core 
{
    public interface IFiniteStateMachineAdvanced<IState, T2, E>
    {
        public Dictionary<E, IState> States { get; set; }
        public IState CurrentState { get; set; }
        public Queue<IEnumerator> Commands { get; set; }
        T2 FSMContext { get; set; }

        public void Initialize(T2 context);
        public void SetState(int id);
        public void SetState(E id);
        public IEnumerator Update();
    }
}