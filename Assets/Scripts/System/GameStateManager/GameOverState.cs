using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine.GameStateManager
{
    public class GameOverState : GameStateManagerBaseState
    {
        private UnityAction<object> returnToMenuAction;

        public GameOverState() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            returnToMenuAction = new UnityAction<object>((object o) => next_state_callback(new MenuState()));
            EventManager.StartListening("ReturnToMenu", returnToMenuAction);

            while (true)
            {
                yield return null;
            }
        }

        public override void EndOfStateCleanup()
        {
            EventManager.StopListening("ReturnToMenu", returnToMenuAction);
        }
    }
}
