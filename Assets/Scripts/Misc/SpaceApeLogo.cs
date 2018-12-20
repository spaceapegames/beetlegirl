using UnityEngine;
using System.Collections;

public class SpaceApeLogo : MonoBehaviour
{
	public GameObject[] logo;
	public AudioSource click;
	public AudioSource spaceApe;
	public Animation fadeOut;
	private float loadtime;

	bool canClickToPlayGame = false;

	void Start ()
	{
		for (int i = 0; i < logo.Length; i++)
		{
			float width = 1 / (float)logo.Length;
			logo[i].GetComponent<Renderer>().material.mainTextureScale = new Vector2(width, 9);
			logo[i].GetComponent<Renderer>().material.mainTextureOffset = new Vector2(width * (float)i , 0);
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)  && canClickToPlayGame)
		{
			LevelTracker levelTracker = FindObjectOfType<LevelTracker>();
			if (levelTracker != null)
				levelTracker.ResetLevel();

			canClickToPlayGame = false;
			loadtime = Time.time + 1.5f;
			fadeOut.Play ();
		}

		if (loadtime > 0 && Time.time > loadtime)
		{
			Application.LoadLevel("LevelSpawner");
		}
	}

	public void PlayClick()
	{
		click.Play();
	}

	public void PlaySpaceApe()
	{
		spaceApe.Play();
	}

	public void FinishAnimation()
	{
		canClickToPlayGame = true;
	}
}
