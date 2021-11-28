using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton

    [SerializeField]
    private Sound[] sounds;

    private void Awake() {
        // Only one AudioManager should be present at all times
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // Copy all sound attributes across to the corresponding sound source
        foreach (Sound sound in sounds) {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.audioClip;
            sound.Source.outputAudioMixerGroup = sound.audioMixerGroup;

            sound.Source.mute = sound.mute;
            sound.Source.bypassEffects = sound.bypassEffects;
            sound.Source.bypassListenerEffects = sound.bypassListenerEffects;
            sound.Source.bypassReverbZones = sound.bypassReverbZones;
            sound.Source.playOnAwake = sound.playOnAwake;
            sound.Source.loop = sound.loop;

            sound.Source.priority = sound.priority;
            sound.Source.volume = sound.volume;
            sound.Source.pitch = sound.pitch;
            sound.Source.panStereo = sound.stereoPan;
            sound.Source.spatialBlend = sound.spatialBlend;
            sound.Source.reverbZoneMix = sound.reverbZoneMix;
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnPlaySound", Play);
        EventManager.StartListening("OnStopSound", Stop);
        EventManager.StartListening("OnMuteSound", Mute);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnPlaySound", Play);
        EventManager.StopListening("OnStopSound", Stop);
        EventManager.StopListening("OnMuteSound", Mute);

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Play(object name) {
        Sound sound = Array.Find(sounds, Sound => Sound.name == (string)name);
        if (sound != null)
            sound.Source.Play();
        else
            Debug.Log("Sound named " + (string)name + " doesn't exist. Can't play sound.");
    }

    public void Stop(object name) {
        Sound sound = Array.Find(sounds, Sound => Sound.name == (string)name);
        if (sound != null)
            sound.Source.Stop();
        else
            Debug.Log("Sound named " + (string)name + " doesn't exist. Can't stop sound.");
    }

    public void Mute(object name) {
        Sound sound = Array.Find(sounds, Sound => Sound.name == (string)name);
        if (sound != null)
            sound.Source.mute = !sound.Source.mute;
        else
            Debug.Log("Sound named " + (string)name + " doesn't exist. Can't mute / unmute.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Stop("BGM");
        switch (scene.name)
        {
            case "MainMenu":
                Play("BGM");
                break;
            
            case "Level 1 - Point A to B":
                Play("BGM");
                break;
            
            case "Level 2 - Survive [X] Time":
                Play("BGM");
                break;
            
            case "Level 3 - Kill All Enemies":
                Play("BGM");
                break;
            
            case "Level 4 - Kill Boss":
                Play("BGM");
                break;
        }
    }
}
