using System;
using UnityEngine;

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
	}

	private void OnDisable()
	{
		EventManager.StopListening("OnPlaySound", Play);
	}

	public void Play(object name) {
		Sound sound = Array.Find(sounds, Sound => Sound.name == (string)name);
		if (sound != null)
			sound.Source.Play();
		else
			Debug.Log("Sound named " + (string)name + " doesn't exist.");
	}
}
