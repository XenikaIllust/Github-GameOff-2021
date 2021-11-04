using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StateMachine;
using StateMachine.GameStateManager;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private List<string> scenes;
    private StateMachine<GameStateManagerBaseState> state;

    private static GameStateManager gameStateManager;
    public static GameStateManager instance
    {
        get
        {
            if (!gameStateManager)
            {
                gameStateManager = FindObjectOfType<GameStateManager>();

                if (!gameStateManager)
                {
                    Debug.Log("There needs to be one active GameStateManager script on a GameObject in your scene.");
                }
                else
                {
                    gameStateManager.Initialize();
                }
            }

            return gameStateManager;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance.WakeUp();
    }

    public void WakeUp() { }

    private void Initialize()
    {
        if (EventManager.instance != null)
        {
            EventManager.StartListening("ChangeScene", ChangeSceneEvent);

            // Initialize everything relating to the state machine
            state = new StateMachine<GameStateManagerBaseState>(this, new GameStateManagerBaseState());
            state.SubscribeStateHasChanged(StateMachine_StateHasChanged);

            state.SetNextState(new MenuState());
            StateMachine_StateHasChanged(new MenuState());
        }
    }

    private void StateMachine_StateHasChanged(BaseState new_state)
    {
        EventManager.RaiseEvent("GameStateChanged", new_state.ToString());
    }

    private void ChangeSceneEvent(object o)
    {
        string nextSceneName = o as string;

        if (scenes.Contains(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError($"Scene [{nextSceneName}] not found.");
        }
    }
}
