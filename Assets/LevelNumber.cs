using UnityEngine;
using System.Collections;

public class LevelNumber : MonoBehaviour
{
	int level = 1;
	SpriteRenderer spriteRenderer;
	public Sprite[] levelNumbers;

	void Start ()
	{
		// TODO: Pull from our global thingy
		LevelTracker levelTracker = FindObjectOfType<LevelTracker>();
		if (levelTracker != null)
			level = levelTracker.level;

		spriteRenderer = GetComponent<SpriteRenderer>();
		if (level >= levelNumbers.Length)
			level = levelNumbers.Length - 1;

		if (spriteRenderer != null)
		{
			spriteRenderer.sprite = levelNumbers[level];
		}
	}
}
