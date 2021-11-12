using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private Sound[] sounds;

	private void Awake () {
		foreach (Sound sound in sounds) {
			sound.Source = gameObject.AddComponent<AudioSource>();
			sound.Source.clip = sound.audioClip;
			sound.Source.volume = sound.Volume;
			sound.Source.pitch = sound.Pitch;
			sound.Source.loop = sound.Loop;
		}
	}

	public void Play (string name) {
		Sound sound = Array.Find(sounds, Sound => Sound.name == name);
		if (sound != null)
			sound.Source.Play();
	}
}
