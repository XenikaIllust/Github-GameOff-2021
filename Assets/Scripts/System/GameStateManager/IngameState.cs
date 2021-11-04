using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine.GameStateManager
{
    public class IngameState : GameStateManagerBaseState
    {
        private UnityAction<object> enterPauseMenuAction;
        private UnityAction<object> enterGameOverAction;
        private UnityAction<object> enterCutsceneAction;

        public IngameState() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            enterPauseMenuAction = new UnityAction<object>((object o) => next_state_callback(new PauseState()));
            EventManager.StartListening("EnterPauseMenu", enterPauseMenuAction);

            enterGameOverAction = new UnityAction<object>((object o) => next_state_callback(new GameOverState()));
            EventManager.StartListening("EnterGameOver", enterGameOverAction);

            enterCutsceneAction = new UnityAction<object>((object o) => next_state_callback(new CutsceneState()));
            EventManager.StartListening("EnterCutscene", enterCutsceneAction);

            while (true)
            {
                yield return null;

                if (Input.GetKeyDown(KeyCode.P))
                {
                    EventManager.RaiseEvent("EnterPauseMenu", null);
                }
            }
        }

        public override void EndOfStateCleanup()
        {
            EventManager.StopListening("EnterPauseMenu", enterPauseMenuAction);
            EventManager.StopListening("EnterGameOver", enterGameOverAction);
            EventManager.StopListening("EnterCutscene", enterCutsceneAction);
        }
    }
}
