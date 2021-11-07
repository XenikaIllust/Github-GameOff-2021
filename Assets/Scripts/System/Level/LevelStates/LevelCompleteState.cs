using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.LevelStateManager
{
    public class LevelCompleteState: LevelStateBaseClass
    {
        public LevelCompleteState() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            Debug.Log("LevelStateManager has entered the victory state!");

            while (true)
            {
                yield return null;
            }
        }
    }
}
