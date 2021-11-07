using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ------------------------
///   End of Level Trigger
/// ------------------------
/// 
/// Sets the level to the victory state when the Player enters the trigger volume.
/// Collider gets disabled in order to prevent entering the state multiple times.
/// </summary>
public class EndOfLevelTrigger : MonoBehaviour
{
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EventManager.RaiseEvent("LevelEvent_CompleteLevel", null);
            if (_collider != null) _collider.enabled = false;
        }
    }
}
