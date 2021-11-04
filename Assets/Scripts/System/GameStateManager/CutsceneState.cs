using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine.GameStateManager
{
    public class CutsceneState : GameStateManagerBaseState
    {
        private UnityAction<object> exitCutsceneAction;

        public CutsceneState() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            exitCutsceneAction = new UnityAction<object>((object o) => next_state_callback(new IngameState()));
            EventManager.StartListening("CutsceneEnded", exitCutsceneAction);

            while (true)
            {
                yield return null;
            }
        }

        public override void EndOfStateCleanup()
        {
            EventManager.StopListening("CutsceneEnded", exitCutsceneAction);
        }
    }
}
