using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability/Ability SO")]
public class AbilitySO : ScriptableObject
{
    public float Duration;
    public float Cooldown;
    public Outcome[] Outcomes;

    public void Execute(object param) {
        foreach(Outcome outcome in Outcomes) {
            // StartCoroutine();
        }
    }

    IEnumerator ExecuteOutcome(Outcome outcome, float timeToExecute) {
        yield return new WaitForSeconds(timeToExecute);

        foreach(GameAction gameAction in outcome.Effects) {
            // gameAction.Invoke(param);
        }
    }
}
