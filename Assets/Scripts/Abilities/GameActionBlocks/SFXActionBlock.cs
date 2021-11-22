using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Definitions/Game Action/SFX/SFXActionBlock")]
public class SFXActionBlock : GameActionBlock
{
    protected const string
        LibraryPrefix = "SFX/AbilitySFX/"; // the prefix of the path to the SFX folder, using Resources.Load

    public override void Invoke(string[] idParams, List<object> currentFilteredTargets,
        Dictionary<string, object> otherTargets)
    {
        EventManager.RaiseEvent("OnPlaySound", idParams[0]);
    }
}