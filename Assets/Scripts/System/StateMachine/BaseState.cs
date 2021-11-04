using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class BaseState
    {
        public virtual IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            while (true)
            {
                yield return null;
            }
        }

        public virtual void EndOfStateCleanup()
        {

        }
    }
}
