using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace StateMachine.GameStateManager
{
    public class PauseState : GameStateManagerBaseState
    {
        private UnityAction<object> exitPauseMenuAction;
        private UnityAction<object> returnToMenuAction;

        public PauseState() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            exitPauseMenuAction = new UnityAction<object>((object o) => next_state_callback(new IngameState()));
            EventManager.StartListening("ExitPauseMenu", exitPauseMenuAction);

            returnToMenuAction = new UnityAction<object>((object o) => next_state_callback(new MenuState()));
            EventManager.StartListening("ReturnToMenu", returnToMenuAction);

            while (true)
            {
                yield return null;

                if (Keyboard.current.pKey.wasPressedThisFrame)
                {
                    EventManager.RaiseEvent("ExitPauseMenu", null);
                }
                else if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    EventManager.RaiseEvent("ReturnToMenu", null);
                }
            }
        }

        public override void EndOfStateCleanup()
        {
            EventManager.StopListening("ExitPauseMenu", exitPauseMenuAction);
            EventManager.StopListening("ReturnToMenu", returnToMenuAction);
        }
    }
}