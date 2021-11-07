using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.LevelStateManager
{
    public class CutsceneState : LevelStateBaseClass
    {
        public CutsceneState() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            Debug.Log("Entered the Cutscene State!");

            while (true)
            {
                yield return null;
            }
        }
    }
}