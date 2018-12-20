using UnityEngine;
using System.Collections;

public class SoundEffects : MonoBehaviour
{
	public static SoundEffectController cont;

	void Start()
	{
		cont = FindObjectOfType<SoundEffectController>();
		if (cont == null)
			Debug.Log ("Shit");
	}

	public static void PlaySound(SoundEffectController.SoundTypes type)
	{
		if (cont != null)
			cont.PlaySound(type);
	}

}
