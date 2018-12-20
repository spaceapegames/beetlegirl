using UnityEngine;
using System.Collections;

public class DoorZone : MonoBehaviour
{
	public GameObject fadeOutAnimation;
	public int flowerCost = 0;
	private float endTime;
	bool isTriggered = false;
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player" && !isTriggered)
		{
			BeetleGirl bg = collider.GetComponentInParent<BeetleGirl>();
			if (bg != null)
			{
				bg.SetMovement(false);
				isTriggered = true;
				endTime = Time.time + 1.5f;
				GameObject go = Instantiate(fadeOutAnimation, Camera.main.transform.position + new Vector3(0, 0, 5), Quaternion.identity) as GameObject;
				go.transform.parent = Camera.main.transform;
				go.GetComponent<Animation>().Play();

				SoundEffects.PlaySound(SoundEffectController.SoundTypes.nice);
				MusicFade music = FindObjectOfType<MusicFade>();
				if (music != null)
					music.DoFade();
			}
		}
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		if (isTriggered && Time.time > endTime)
		{
			Debug.Log ("(✿◕‿◕) Who wants da reeeeeewind");
			LevelTracker levelTracker = FindObjectOfType<LevelTracker>();
			if (levelTracker)
			{
				levelTracker.IncreaseLevel();
			}

			Application.LoadLevel ("LevelSpawner");
		}
		
	}
}
