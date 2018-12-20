using UnityEngine;
using System.Collections;

public class BeetleWrangler : MonoBehaviour
{
	public GameObject beetleGirlPrefab;

	private StartZone startZone;
	private EndZone endZone;
	private BeetleGirl beetleGirl;
	private CameraTrack cameraTrack;
	private bool spawning;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(startZone == null)
		{
			startZone = transform.parent.GetComponentInChildren<StartZone>();
		}

		if(endZone == null)
		{
			endZone = transform.parent.GetComponentInChildren<EndZone>();
		}

		if(cameraTrack == null)
		{
			cameraTrack = transform.parent.GetComponentInChildren<CameraTrack>();
		}

		if (startZone != null && beetleGirl == null)// && Input.GetKeyDown(KeyCode.A) && Application.isEditor)
		{
			SpawnBeetleGirl();
		}

		if(beetleGirl != null && Input.GetKeyDown(KeyCode.Q) && Application.isEditor)
		{
			beetleGirl.Kill();
		}
	}

	private void SpawnBeetleGirl()
	{
		if(!spawning)
		{
			StartCoroutine("SpawnHer");
		}
	}

	private void BeetleStateChanged(BeetleGirl.BeetleState state)
	{
		if(state != BeetleGirl.BeetleState.pathing)
		{
			// Argh. Ded.
			StartCoroutine("FinishHer");
		}
	}

	private IEnumerator SpawnHer()
	{
		spawning = true;
		yield return new WaitForSeconds(2f); 

		var girl = Object.Instantiate(beetleGirlPrefab);
		girl.name = "BeetleGirl";
		girl.transform.SetParent(transform);
		girl.transform.position = startZone.spawnPoint.transform.position;
		
		beetleGirl = girl.GetComponent<BeetleGirl>();
		beetleGirl.OnStateChanged += BeetleStateChanged;
		
		cameraTrack.AssignBeetleGirl(beetleGirl);
	}

	private IEnumerator FinishHer()
	{
		yield return new WaitForSeconds(2f); 
		/*if(beetleGirl != null)
		{
			beetleGirl.OnStateChanged -= BeetleStateChanged;
			Destroy(beetleGirl.gameObject);
			beetleGirl = null;
			spawning = false;
		}*/
	}
}
