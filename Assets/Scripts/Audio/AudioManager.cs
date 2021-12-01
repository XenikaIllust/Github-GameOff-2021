using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton

    [SerializeField]
    private Sound[] sounds;

    private string currentBGM;

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

    private void Play(object name) {
        Sound sound = Array.Find(sounds, Sound => Sound.name == (string)name);
        if (sound != null)
            sound.Source.Play();
        else
            Debug.Log("Sound named " + (string)name + " doesn't exist. Can't play sound.");
    }

    private void Stop(object name) {
        Sound sound = Array.Find(sounds, Sound => Sound.name == (string)name);
        if (sound != null)
            sound.Source.Stop();
        else
            Debug.Log("Sound named " + (string)name + " doesn't exist. Can't stop sound.");
    }

    private void Mute(object name) {
        Sound sound = Array.Find(sounds, Sound => Sound.name == (string)name);
        if (sound != null)
            sound.Source.mute = !sound.Source.mute;
        else
            Debug.Log("Sound named " + (string)name + " doesn't exist. Can't mute / unmute.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                Stop(currentBGM);
                currentBGM = "Main Menu BGM";
                Play(currentBGM);
                break;
            
            case "Level 1 - Point A to B variant 0":
                Stop(currentBGM);
                currentBGM = "Battle BGM " + UnityEngine.Random.Range(0, 4).ToString();
                Play(currentBGM);
                break;
            
            case "Level 2 - Survive [X] Time":
                Stop(currentBGM);
                currentBGM = "Battle BGM " + UnityEngine.Random.Range(0, 4).ToString();
                Play(currentBGM);
                break;
            
            case "Level 3 - Kill All Enemies":
                Stop(currentBGM);
                currentBGM = "Battle BGM " + UnityEngine.Random.Range(0, 4).ToString();
                Play(currentBGM);
                break;

            case "Level 4 - Kill Elite variant 1":
                Stop(currentBGM);
                currentBGM = "Battle BGM " + UnityEngine.Random.Range(0, 4).ToString();
                Play(currentBGM);
                break;

            case "Level 5 - Kill Claire":
                Stop(currentBGM);
                currentBGM = "Boss Battle";
                Play(currentBGM);
                break;
            
            case "Credits Scene":
                Stop(currentBGM);
                currentBGM = "Credits";
                Play(currentBGM);
                break;
        }
    }
}
