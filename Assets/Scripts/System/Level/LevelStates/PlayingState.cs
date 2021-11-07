using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.LevelStateManager
{
    public class PlayingState : LevelStateBaseClass
    {
        public PlayingState() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            Debug.Log("Entered the Ingame State!");

            while (true)
            {
                yield return null;
            }
        }
    }
}
