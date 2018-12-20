using UnityEngine;
using System.Collections;

public class SoundEffectController : MonoBehaviour
{
	public enum SoundTypes
	{
		bump, death, landing, go, nice, sneeze, yay
	}

	// This is not the way you should do this
	public AudioClip[] bumpSounds;
	public AudioClip[] deathSounds;
	public AudioClip[] landingSounds;
	public AudioClip[] goSounds;
	public AudioClip[] niceSounds;
	public AudioClip[] sneezeSounds;
	public AudioClip[] yaySounds;

	public void PlaySound(SoundTypes type)
	{
		AudioClip[] sounds = new AudioClip[0];

		switch (type)
		{
		case SoundTypes.bump:
			sounds = bumpSounds;
			break;
		case SoundTypes.death:
			sounds = deathSounds;
			break;
		case SoundTypes.landing:
			sounds = landingSounds;
			break;
		case SoundTypes.go:
			sounds = goSounds;
			break;
		case SoundTypes.nice:
			sounds = niceSounds;
			break;
		case SoundTypes.sneeze:
			sounds = sneezeSounds;
			break;
		case SoundTypes.yay:
			sounds = yaySounds;
			break;
		}

		if (sounds.Length > 0)
		{
			AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Length)], transform.position);
		}

	}

	void Update()
	{
		if (!Application.isEditor)
			return;

		// Nasrin sound board
		if (Input.GetKeyDown(KeyCode.Alpha1))
			PlaySound(SoundTypes.bump);
		if (Input.GetKeyDown(KeyCode.Alpha2))
			PlaySound(SoundTypes.death);
		if (Input.GetKeyDown(KeyCode.Alpha3))
			PlaySound(SoundTypes.landing);
		if (Input.GetKeyDown(KeyCode.Alpha4))
			PlaySound(SoundTypes.go);
		if (Input.GetKeyDown(KeyCode.Alpha5))
			PlaySound(SoundTypes.nice);
		if (Input.GetKeyDown(KeyCode.Alpha6))
			PlaySound(SoundTypes.sneeze);
		if (Input.GetKeyDown(KeyCode.Alpha7))
			PlaySound(SoundTypes.yay);
	}
}
