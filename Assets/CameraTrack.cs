using UnityEngine;
using System.Collections;

public class CameraTrack : MonoBehaviour
{
	private BeetleGirl beetleGirl;
	private bool inSlowMode = false;
	public GameObject killzone;

	private const float normalSpeed = 1f;
	private const float slowSpeedFactor = 0.50f;
	private const float startSlowRatio = 0.20f;
	private const float endSlowRatio = 0.30f;
	private const float matchSpeedRatio = 0.50f;
	private float startSlowDistance;
	private float endSlowDistance;
	private float matchSpeedDistance;

	public void AssignBeetleGirl (BeetleGirl ourPrideAndJoy)
	{
		beetleGirl = ourPrideAndJoy;
		SoundEffects.PlaySound(SoundEffectController.SoundTypes.go);
	}

	void Start ()
	{
		BeetleGirl searchedBeetleGirl = FindObjectOfType<BeetleGirl>();
		if (searchedBeetleGirl != null)
			beetleGirl = searchedBeetleGirl;
		else
			Debug.Log("(✿◕‿◕) No Beetle Girl found in scene. But it's never too late to AssignBeetleGirl()");

		if (killzone != null)
		{
			Vector3 killzonePosition = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height / 2));
			killzonePosition.z = 0;
			killzonePosition.x -= 0.5f;
			killzone.transform.position = killzonePosition;
		}

		// Distances
		startSlowDistance = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * startSlowRatio, 0)).x;
		endSlowDistance = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * endSlowRatio, 0)).x;
		matchSpeedDistance = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * matchSpeedRatio, 0)).x;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (beetleGirl == null || beetleGirl.state != BeetleGirl.BeetleState.pathing)
			return;
	
		float speed = normalSpeed * Time.deltaTime;
		float x = beetleGirl.transform.position.x;

		if (inSlowMode && x > transform.position.x + endSlowDistance)
		{
			inSlowMode = false;
			Debug.Log ("(✿◕‿◕) No longer in slow mode");
		}
		else if (x < transform.position.x + startSlowDistance || inSlowMode)
		{
			if (inSlowMode == false)
				Debug.Log ("(✿◕‿◕) Entering slow mode");
			inSlowMode = true;
			speed *= slowSpeedFactor;
		}

		if (x > transform.position.x + matchSpeedDistance)
		{
			speed = BeetleGirl.maxSpeed * Time.deltaTime;
			//Debug.Log ("(✿◕‿◕) Matching beetle girl speed");
		}

		transform.Translate(speed, 0, 0);
	}
}
