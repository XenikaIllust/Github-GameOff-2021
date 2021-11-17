using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/SFX/SFXActionBlock")]
public class SFXActionBlock : GameActionBlock
{
    public override void Invoke(string[] idParams, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets) {
        EventManager.RaiseEvent("OnPlaySound", idParams[0]);
    }
}
