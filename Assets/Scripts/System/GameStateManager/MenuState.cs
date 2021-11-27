using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace StateMachine.GameStateManager
{
    public class MenuState : GameStateManagerBaseState
    {
        private UnityAction<object> enterGameActionBlock;
        private UnityAction<object> enterPauseMenuAction;

        public MenuState() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            enterGameActionBlock = new UnityAction<object>((object o) => next_state_callback(new IngameState()));
            EventManager.StartListening("StartGame", enterGameActionBlock);

            enterPauseMenuAction = new UnityAction<object>((object o) => next_state_callback(new PauseState()));
            EventManager.StartListening("PauseGame", enterPauseMenuAction);

            while (true)
            {
                yield return null;

                if (Keyboard.current.gKey.wasPressedThisFrame)
                {
                    EventManager.RaiseEvent("StartGame", null);
                }
                else if (Keyboard.current.pKey.wasPressedThisFrame)
                {
                    EventManager.RaiseEvent("PauseGame", null);
                }
            }
        }

        public override void EndOfStateCleanup()
        {
            EventManager.StopListening("StartGame", enterGameActionBlock);
            EventManager.StopListening("PauseGame", enterPauseMenuAction);
        }
    }
}
