using UnityEngine;
using System.Collections;
using System;

public class FlowerCollectable : MonoBehaviour
{
	public GameObject VFXcollect;
	bool collected = false;
	private MeshRenderer mesh;

	public Action OnFlowerCollected;

	void Start()
	{
		mesh = GetComponentInChildren<MeshRenderer>();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player" && !collected)
		{
			BeetleGirl bg = collider.GetComponentInParent<BeetleGirl>();
			if (bg != null)
			{
				RemoveFlower();

				if (UnityEngine.Random.Range(0, 100) < 10)
					SoundEffects.PlaySound(SoundEffectController.SoundTypes.sneeze);
				else
					SoundEffects.PlaySound(SoundEffectController.SoundTypes.yay);

				if (VFXcollect)
					Instantiate (VFXcollect, transform.position, Quaternion.identity);
			}
		}
	}

	public void RemoveFlower()
	{
		if(collected)
		{
			return;
		}
		collected = true;

		if(OnFlowerCollected != null)
		{
			OnFlowerCollected();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (mesh != null && collected)
		{
			mesh.enabled = false;
		}
	}
}
