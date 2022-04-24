using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Tooltip("The sound and music sources")]
    [SerializeField] private AudioSource[] audioSources;

	private AudioSource soundSource;
	private AudioSource musicSource;

	private Dictionary <AudioClipName, AudioClip> _audioClipsDictionary = new Dictionary<AudioClipName, AudioClip>();
	private void Awake()
    {
		audioSources = GetComponentsInParent<AudioSource>();

		soundSource = audioSources[0];
		musicSource = audioSources[1];

		soundSource.volume = 0.8f;
		musicSource.volume = 0.4f;

		Initialized();
	}
    private void Initialized()
    {
		_audioClipsDictionary.Add(AudioClipName.MainTheme, Resources.Load<AudioClip>("MainTheme"));
		_audioClipsDictionary.Add(AudioClipName.FailedSound, Resources.Load<AudioClip>("FailedSound"));
		_audioClipsDictionary.Add(AudioClipName.WinSound, Resources.Load<AudioClip>("WinSound"));
		_audioClipsDictionary.Add(AudioClipName.UISound, Resources.Load<AudioClip>("UISound"));
	}

    public void PlaySound(AudioClipName clip)
	{
		soundSource.PlayOneShot(_audioClipsDictionary[clip]);
	}
	public void PlayMusic(AudioClipName clip)
	{
		musicSource.clip = _audioClipsDictionary[clip];
		musicSource.Play();
	}
}
