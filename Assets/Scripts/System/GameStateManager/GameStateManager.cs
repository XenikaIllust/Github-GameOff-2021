using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StateMachine;
using StateMachine.GameStateManager;

/// <summary>
/// ------------------------
///    Game State Manager
/// ------------------------
/// 
/// Handles code relating to game flow and scene management.
/// </summary>
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

    /// <summary>
    /// This is called in order to initialize our singleton instance.
    /// </summary>
    public void WakeUp() { }

    /// <summary>
    /// Hooks our events into the EventManager so we can be notified later.
    /// </summary>
    private void Initialize()
    {
        if (EventManager.Instance != null)
        {
            EventManager.StartListening("ChangeScene", ChangeSceneEvent);

            InitializeStateMachine();         
        }
    }

    /// <summary>
    /// Performs initialization actions on the state machine.
    /// </summary>
    private void InitializeStateMachine()
    {
        state = new StateMachine<GameStateManagerBaseState>(this, new MenuState());

        state.SubscribeStateHasChanged(StateMachine_StateHasChanged);
        StateMachine_StateHasChanged(new MenuState());
    }

    /// <summary>
    /// Callback function for when the state machine changes state.
    /// </summary>
    /// <param name="new_state">The state being transitioned to.</param>
    private void StateMachine_StateHasChanged(BaseState new_state)
    {
        EventManager.RaiseEvent("GameStateChanged", new_state);
    }

    /// <summary>
    /// Callback function for when we want to change the current scene.
    /// </summary>
    /// <param name="o">Should be the name of the next scene.</param>
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
