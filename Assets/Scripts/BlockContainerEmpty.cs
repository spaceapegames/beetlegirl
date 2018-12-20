using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockContainerEmpty : MonoBehaviour
{
	private Vector3 ScreenSpace;
	//private Vector3 Offset;

	private List<Block> Blocks = new List<Block>();

	private List<Block> BlockPool = new List<Block>();

	private Vector3 lastPos;

	private bool immovable = false;

	private BoxCollider boxCollider;

	public static int MAX_BLOCKS = 5;

	public int placement = 2;

	// Use this for initialization
	void Start ()
	{
		boxCollider = GetComponent<BoxCollider>();
		boxCollider.size = new Vector3(1f,MAX_BLOCKS,2f);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void AddBlock(BlockType type)
	{
		if(Blocks.Count < MAX_BLOCKS)
		{
			Debug.Log("Adding Block " + type.ToString());

			var prefab = LevelSpawner.prefabByType[type];
			if(prefab != null)
			{
				var clone = Object.Instantiate(prefab);
				clone.name = "Block " + (Blocks.Count + 1).ToString();
				clone.transform.SetParent(transform);
				clone.transform.localPosition = new Vector3(0,placement,0);

				var block = clone.GetComponent<Block>();
				block.BlockType = type;
				block.OnBlockCollection += OnCollected;
				Blocks.Add(block);

				if(type == BlockType.Immovable || type == BlockType.Fire)
				{
					immovable = true;
				}

				PoolClone(block);

				placement -= 1;
			}
		}
	}

	void FixedUpdate()
	{
		if(CanBlockBeeSeen(boxCollider.bounds))
		{
			var toPool = new List<Block>();
			var toAdd = new List<Block>();

			foreach(var block in Blocks)
			{
				if(block.BoxCollider != null)
				{
					var min = block.BoxCollider.bounds.min;
					var max = block.BoxCollider.bounds.max;

					if(!boxCollider.bounds.Contains(min) || !boxCollider.bounds.Contains(max))
					{
						var diff = (block.transform.position - transform.position).y;

						var duplicate = GetBlockOfType(block);
						if(duplicate != null)
						{
							var newPos = block.transform.position;

							float offset = 0;
							if(diff > 0)
							{
								offset = diff - boxCollider.size.y;
							}
							else if (diff < 0)
							{
								offset = diff + boxCollider.size.y;
							}

							duplicate.gameObject.SetActive(true);
							duplicate.transform.position = new Vector3(newPos.x,offset,newPos.z);
							toAdd.Add(duplicate);
						}
					}

					// The Ugliest Hack in Hackerdom
					var extents = new Bounds();
					extents.center = transform.position;
					extents.extents = new Vector3(0.5f,0.5f,0.5f);

					if(!CanBlockBeeSeen(extents))
					{
						block.gameObject.SetActive(false);
						toPool.Add(block);
					}
				}
			}

			foreach(var block in toPool)
			{
				Blocks.Remove(block);
			}
			BlockPool.AddRange(toPool);
			Blocks.AddRange(toAdd);
		}
	}

	private void PoolClone(Block block)
	{
		var prefab = LevelSpawner.prefabByType[block.BlockType];
		if(prefab != null)
		{
			var clone = Object.Instantiate(prefab);
			clone.name = block.gameObject.name;
			clone.transform.SetParent(transform);
			clone.transform.localPosition = new Vector3(0,0,0);
			clone.gameObject.SetActive(false);

			var newBlock = clone.GetComponent<Block>();
			BlockPool.Add(newBlock);
		}
	}

	private Block GetBlockOfType(Block block)
	{
		foreach(var pooled in BlockPool)
		{
			if(block.gameObject.name == pooled.gameObject.name)
			{
				BlockPool.Remove(pooled);
				return pooled;
			}
		}
		return null;
	}

	private bool CanBlockBeeSeen(Bounds collider)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

		if (GeometryUtility.TestPlanesAABB(planes, collider))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void OnMouseDown()
	{
		if(!immovable)
		{
			ScreenSpace = Camera.main.WorldToScreenPoint (transform.position);
			lastPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenSpace.z));
			//Offset = transform.position - lastPos;
		}
	} 
	
	void OnMouseDrag()
	{
		if(!immovable)
		{
			var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenSpace.z); 
			var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace);// + Offset;

			//var differenceY = curPosition.y - transform.position.y;
			var difference = (curPosition - lastPos).y;
			lastPos = curPosition;

			foreach(var child in Blocks)
			{
				child.MoveY(difference);
			}
		}
	}

	private void OnCollected(Block block)
	{
		foreach(var pooled in Blocks)
		{
			if(block.gameObject.name == pooled.gameObject.name)
			{
				pooled.RemoveCollectible();
			}
		}
		foreach(var pooled in BlockPool)
		{
			if(block.gameObject.name == pooled.gameObject.name)
			{
				pooled.RemoveCollectible();
			}
		}
	}

	void OnDestroy()
	{
		foreach(var pooled in Blocks)
		{
			pooled.OnBlockCollection -= OnCollected;
		}
		foreach(var pooled in BlockPool)
		{
			pooled.OnBlockCollection -= OnCollected;
		}
		Blocks.Clear();
		BlockPool.Clear();
	}
}
