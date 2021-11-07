using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.LevelStateManager
{
    public class LoadLevelState : LevelStateBaseClass
    {
        public LoadLevelState() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            yield return null;

            Debug.Log("Entered Load Level State!");
            EventManager.RaiseEvent("LevelEvent_EnterIngameState", null);

            while (true)
            {
                yield return null;
            }
        }
    }
}
