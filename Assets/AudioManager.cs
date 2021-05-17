using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Audio manager script taken from:
    // https://www.youtube.com/watch?v=6OT43pvUyfY&ab_channel=Brackeys

    public float fadeTime;

    public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;



	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
            s.maxVolume = s.source.volume;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

    public void FadeTheSound(string sound, bool fadeIn)
    {
        StartCoroutine(FadeSound(sound, fadeIn));
    }

    IEnumerator FadeSound(string sound, bool fadeIn)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            yield break;
        }
        

        if (fadeIn)
        {
            Debug.Log("Fade in wind");
            s.source.volume = 0;
            
            while (s.source.volume < s.maxVolume)
            {
                s.source.volume += (0.01f);
                yield return null;
            }
            
        }
        else
        {
            while (s.source.volume > 0)
            {
                s.source.volume -= (Time.deltaTime / fadeTime);
                yield return null;
            }
        }
    }
}
