using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// -----------------
    ///   State Machine
    /// -----------------
    /// 
    /// Generic implementation of a state machine. Uses coroutines to handle state processing.
    /// 
    /// HOW TO USE:
    /// ----------------------
    /// 
    /// If you'd like to use a state machine in a class, add the following using directives:
    /// 
    /// using StateMachine;
    /// using StateMachine.[NameOfMachineToUse]; // If applicable
    /// 
    /// At some point during your script, either at Awake or Start, you'll need to instantiate the state machine
    /// in order to get it to run. The state you provide in the constructor is what state the machine will run first.
    /// So for example, if you have a state machine for a player class it would look like:
    /// 
    /// playerStateMachine = new StateMachine<PlayerBaseState>(this, new IdleState());
    /// 
    /// This will automatically start the processing coroutine and, in the case of the above example, start our player
    /// in the Idle state.
    /// 
    /// To change the state of a state machine, you must instantiate a new state type of the state you'd like to enter.
    /// To use our player example again, this could look like:
    /// 
    /// playerStateMachine.SetNextState(new Jump());
    /// playerStateMachine.SetNextState(new Attack(target, damage);
    /// 
    /// To be notified of state changes, you'll need to subscribe to the StateHasChanged delegate.
    /// StateHasChanged's method signature is a void that takes in a BaseState, and the event fires any time SetNextState
    /// is called.
    /// 
    /// HOW TO CREATE A NEW STATE MACHINE:
    /// --------------------------------------------
    /// 
    /// All state machines should be placed inside of their own namespaces. This will prevent conflicts with duplicate class
    /// names across multiple state machines. This statemachine already exists inside of the StateMachine namespace, so consider
    /// placing it in a sub-directory of that. For example: namespace StateMachine.Player, or namespace StateMachine.LevelManager
    /// 
    /// Each state must derive from BaseState, or whichever derived class your particular state machine uses as a base state.
    /// States are broken up into three separate sections. They are as follows:
    /// 
    /// CONSTRUCTOR - The class constructors act as an entry point, naturally, for all states. The default state
    /// constructor takes no parameters, but derived states can define a list of parameters in order to pass along additional
    /// information.
    /// 
    /// PROCESS_STATE - The main function that handles a state's code. This is a coroutine, so be sure to structure your code accordingly.
    /// The coroutine takes in two parameters; subject, which is the object this particular state machine belongs to (ex. the player, an enemy),
    /// and next_state_callback, which is a delegate that points to the state machine's SetNextState function. If you want to define a state
    /// transition inside of ProcessState, just call it like so:
    /// 
    /// next_state_callback(new StateToTransitionTo());
    /// 
    /// When working with the subject parameter, you'll need to cast it to whatever type it is in order to use its values. This is due
    /// to the fact that subject is passed as a MonoBehaviour. So for example, if we were building a state machine for an object of type
    /// PlayerScript and were building a healing state, we'd need to do the following:
    /// 
    /// public override void ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
    /// {
    ///     var player = subject as PlayerScript;
    ///     
    ///     if (player != null)
    ///     {
    ///         player.health += 1;
    ///         yield return null;
    ///     }
    /// }
    /// 
    /// END_OF_STATE_CLEANUP - Automatically called when a state transition happens. If your state subscribes to any delegates, this
    /// is the place where you'd unsubscribe.
    /// 
    /// KNOWN ISSUES
    /// --------------------
    /// 
    /// This is certainly not a perfect implementation of a FSM, and there are a couple of common situations that can cause problems
    /// in your game. I'll try to list them and their workarounds below.
    /// 
    /// 1. If two state transitions happen in a single update cycle, the state machine will break completely. This can be avoided by
    /// putting a yield return null statement at the beginning of your ProcessState coroutine. For whatever reason the state machine
    /// needs at least a single frame delay inbetween states in order to function properly.
    /// 
    /// 2. Make sure the state coroutines don't end! This can potentially cause some issues. If you need a state to
    /// just sit and wait for something, consider adding a while(true) loop (just make sure there's a yield return null in there though).
    /// 
    /// 3. A state machine's coroutines run on the MonoBehaviour that they're attached to, so if you call StopAllCoroutines(), then
    /// this will also break the currently running state machine. If this happens, it's possible to restart the state machine by
    /// reinstantiating it.
    /// 
    /// </summary>
    public class StateMachine<T> where T : BaseState
    {
        private MonoBehaviour subject; // The object this state machine is attached to. For getting and setting values.
        private T base_state; // A generic state for a given state machine
        private T current_state;
        private Coroutine active_state_routine; // The currently running state

        #region Delegates and subscriber methods

        private System.Action<BaseState> StateHasChanged; // Fires whenever the state is changing
        public void SubscribeStateHasChanged(System.Action<BaseState> sub) { StateHasChanged += sub; }
        public void UnsubscribeStateHasChanged(System.Action<BaseState> sub) { StateHasChanged -= sub; }

        #endregion

        public StateMachine(MonoBehaviour m_subject, T m_base_state)
        {
            subject = m_subject;
            base_state = m_base_state;

            subject.StartCoroutine(ProcessStateMachine());
        }

        public BaseState GetCurrentState()
        {
            return current_state;
        }

        public void SetNextState(BaseState received_state)
        {
            current_state.EndOfStateCleanup();
            current_state = (T)received_state;

            if (active_state_routine != null)
            {
                subject.StopCoroutine(active_state_routine);
                active_state_routine = null;
            }
        }

        public IEnumerator ProcessStateMachine()
        {
            current_state = base_state;

            while (true)
            {
                if (active_state_routine == null)
                {
                    active_state_routine = subject.StartCoroutine(current_state.ProcessState(subject, SetNextState));
                    StateHasChanged?.Invoke(current_state);
                }

                yield return null;
            }
        }
    }
}