using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachine;
using StateMachine.LevelStateManager;

/// <summary>
/// ------------------------
///   Level Event Manager
/// ------------------------
/// 
/// Handles the level state and associated event calls.
/// Currently, all event calls for this class will only modify the manager's current state.
/// </summary>
public class LevelEventManager : MonoBehaviour
{
    // Components
    private StateMachine<LevelStateBaseClass> stateMachine;

    // Delegates and initialization
    private bool hasBeenInitialized = false;
    private UnityAction<object> LoadLevel;
    private UnityAction<object> EnterIngameState;
    private UnityAction<object> EnterCutsceneState;
    private UnityAction<object> CompleteLevel;

    // Level flow variables
    private bool isLevelComplete = false;

    private void Awake()
    {
        // Initialize components
        stateMachine = new StateMachine<LevelStateBaseClass>(this, new LoadLevelState());
    }

    /// <summary>
    /// Populate our delegates with the actions we want each event to take.
    /// This method should only be called once.
    /// </summary>
    private void InitializeLevelStateEvents()
    {
        hasBeenInitialized = true;

        LoadLevel = new UnityAction<object>((object o) => stateMachine.SetNextState(new LoadLevelState()));
        EnterIngameState = new UnityAction<object>((object o) => stateMachine.SetNextState(new PlayingState()));
        EnterCutsceneState = new UnityAction<object>((object o) => stateMachine.SetNextState(new CutsceneState()));

        CompleteLevel = new UnityAction<object>((object o) =>
        {
            if (!isLevelComplete)
            {
                stateMachine.SetNextState(new LevelCompleteState());
                isLevelComplete = true;
            }
        });
    }

    /// <summary>
    /// Subscribe our listeners to the EventManager whenever the manager GameObject is activated / reactivated.
    /// If the script is attached to an object in the scene, this will be called immediately at the start of the scene.
    /// 
    /// Please follow the format of 'LevelEvent_DelegateName' when adding new events.
    /// </summary>
    private void OnEnable()
    {
        if (!hasBeenInitialized)
        {
            InitializeLevelStateEvents();
        }

        EventManager.StartListening("LevelEvent_LoadLevel", LoadLevel);
        EventManager.StartListening("LevelEvent_EnterIngameState", EnterIngameState);
        EventManager.StartListening("LevelEvent_EnterCutsceneState", EnterCutsceneState);
        EventManager.StartListening("LevelEvent_CompleteLevel", CompleteLevel);
    }

    /// <summary>
    /// Unsubscribes our listeners from the EventManager whenever the manager is disabled / destroyed.
    /// </summary>
    private void OnDisable()
    {
        EventManager.StopListening("LevelEvent_LoadLevel", LoadLevel);
        EventManager.StopListening("LevelEvent_EnterIngameState", EnterIngameState);
        EventManager.StopListening("LevelEvent_EnterCutsceneState", EnterCutsceneState);
        EventManager.StopListening("LevelEvent_CompleteLevel", CompleteLevel);
    }
}
