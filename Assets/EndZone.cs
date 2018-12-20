using UnityEngine;
using System.Collections;
using System;

public class EndZone : MonoBehaviour
{
	public Goal goal;

	public bool complete;
	public Action OnGoalReached;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEntered(Collider other)
	{
		if(!complete)
		{
			complete = true;
			if(OnGoalReached != null)
			{
				OnGoalReached();
			}
		}
	}
}
