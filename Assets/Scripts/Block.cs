using UnityEngine;
using System.Collections;
using System;

public class Block : MonoBehaviour
{
	public int Direction { get; private set; }
	public bool Cloned { get; set; }

	public BlockType BlockType { get; set; }

	public BoxCollider BoxCollider;
	public FlowerCollectable Flower;

	public Action<Block> OnBlockCollection;

	// Use this for initialization
	void Start ()
	{
		BoxCollider = GetComponent<BoxCollider>();
		Flower = GetComponent<FlowerCollectable>();
		if(Flower != null)
		{
			Flower.OnFlowerCollected += FlowerCollected;
		}
	}

	void OnDestroy()
	{
		if(Flower != null)
		{
			Flower.OnFlowerCollected -= FlowerCollected;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void MoveY (float differenceY)
	{
		if(differenceY > 0)
		{
			Direction = 1;
		}
		else if(differenceY < 0)
		{
			Direction = -1;
		}
		else
		{
			Direction = 0;
		}

		var curPos = transform.position;
		var newPos = new Vector3(curPos.x,curPos.y + differenceY,curPos.z);
		transform.position = newPos;
	}

	public void RoundY ()
	{
		Direction = 0;
		var roundedY = Mathf.CeilToInt(transform.position.y);
		var newPos = new Vector3(transform.position.x,roundedY,transform.position.z);
		transform.position = newPos;
	}

	public void RemoveCollectible()
	{
		if(Flower == null)
		{
			Flower = GetComponent<FlowerCollectable>();
		}
		if(Flower != null)
		{
			Flower.RemoveFlower();
		}
	}

	private void FlowerCollected()
	{
		if(OnBlockCollection != null)
		{
			OnBlockCollection(this);
		}
	}
}
