using UnityEngine;
using System.Collections;

public class LevelTracker : MonoBehaviour {

	public int level { private set; get; }

	public void ResetLevel()
	{
		level = 1;
	}


	public void IncreaseLevel()
	{
		level++;
	}

	void Start ()
	{
		level = 1;

		// Highlander
		LevelTracker[] leveltrackers = FindObjectsOfType<LevelTracker>();
		foreach (LevelTracker t in leveltrackers)
		{
			if (t != this)
			{
				Destroy(gameObject);
			}
		}

		DontDestroyOnLoad(this);
	}
}
