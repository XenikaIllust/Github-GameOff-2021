using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine<T> where T : BaseState
    {
        private MonoBehaviour subject;
        private T base_state;
        private T current_state;
        private Coroutine active_state_routine;

        private System.Action<BaseState> StateHasChanged;
        public void SubscribeStateHasChanged(System.Action<BaseState> sub) { StateHasChanged += sub; }
        public void UnsubscribeStateHasChanged(System.Action<BaseState> sub) { StateHasChanged -= sub; }

        public StateMachine(MonoBehaviour m_subject, T m_base_state)
        {
            subject = m_subject;
            base_state = m_base_state;

            subject.StartCoroutine(ProcessStateMachine());
        }

        public BaseState GetCurrentState()
        {
            return current_state;
        }

        public void SetNextState(BaseState received_state)
        {
            current_state.EndOfStateCleanup();
            current_state = (T)received_state;

            if (active_state_routine != null)
            {
                subject.StopCoroutine(active_state_routine);
                active_state_routine = null;
            }
        }

        public IEnumerator ProcessStateMachine()
        {
            current_state = base_state;

            while (true)
            {
                if (active_state_routine == null)
                {
                    active_state_routine = subject.StartCoroutine(current_state.ProcessState(subject, SetNextState));
                    StateHasChanged?.Invoke(current_state);
                }

                yield return null;
            }
        }
    }
}