using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class MusicFade : MonoBehaviour
{
	float pitch = 1.0f;
	bool doFade = false;
	AudioSource audioSource;

	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			Debug.Log ("(✿◕‿◕) Music Fader has no Audio Source");
	}

	void Update()
	{
		if (doFade)
		{
			pitch = Mathf.Clamp (pitch - (0.75f * Time.deltaTime), 0, 1);
			audioSource.pitch = pitch;
			if (pitch <= 0)
			{
				audioSource.Stop ();
				doFade = false;
			}
		}
	}
	
	public void DoFade()
	{
		if (pitch > 0)
			doFade = true;
	}
}
