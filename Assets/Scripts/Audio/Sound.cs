using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip audioClip;
    public AudioMixerGroup audioMixerGroup;

    public bool mute;
    public bool bypassEffects;
    public bool bypassListenerEffects;
    public bool bypassReverbZones;
    public bool playOnAwake;
    public bool loop;

    [Range(0, 256)]
    public int priority;
    [Range(0.0f,1.0f)]
    public float volume = 1.0f;
    [Range(0.1f, 3.0f)]
    public float pitch = 1.0f;
    [Range(-1.0f, 1.0f)]
    public float stereoPan;
    [Range(0.0f, 1.0f)]
    public float spatialBlend;
    [Range(0.0f, 1.1f)]
    public float reverbZoneMix;

    [HideInInspector]
    public AudioSource Source;
}
