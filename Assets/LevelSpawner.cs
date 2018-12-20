using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public enum BlockType
{
	Air,
	Moveable,
	Immovable,
	Spike,
	Fire,
	Flower
}

public class LevelSpawner : MonoBehaviour
{
	public Animation fadeInAnimation;
	public GameObject startPrefab;
	public GameObject endPrefab;
	public GameObject blockContainerPrefab;

	public GameObject air;
	public GameObject moveable;
	public GameObject immoveable;
	public GameObject spike;
	public GameObject fire;
	public GameObject flower;

	private List<BlockContainerEmpty> columns = new List<BlockContainerEmpty>();

	public static Dictionary<BlockType,GameObject> prefabByType = new Dictionary<BlockType,GameObject>();

	private LevelSegments levelSegments;

	private const string LEVEL_FILE = "bestLevel";

	// Use this for initialization
	void Start ()
	{
		// Whooosh
		fadeInAnimation.Play ();

		RegisterPrefabs();

		var start = Object.Instantiate(startPrefab);
		start.name = "Start";
		start.transform.SetParent(transform);
		start.transform.position = new Vector3(0,0,0);

		float location = 2.5f;
		LoadFile(out location);
		//DebugColumnRiot(out location);

		location += 1.5f;
		var end = Object.Instantiate(endPrefab);
		end.name = "Start";
		end.transform.SetParent(transform);
		end.transform.position = new Vector3(location,0,0);
	}

	private void DebugColumnRiot(out float location)
	{
		location = 2.5f;
		for(int i = 0; i < 5; ++i)
		{
			AddFillerColumn(location, "Column " + (i+1).ToString());
			location++;
		}
	}

	private void AddFillerColumn(float location, string nameOveride = "Filler Column")
	{
		var clone = Object.Instantiate(blockContainerPrefab);
		clone.name = nameOveride;
		clone.transform.SetParent(transform);
		clone.transform.position = new Vector3(location,0,0);
		
		var column = clone.GetComponent<BlockContainerEmpty>();
		columns.Add(column);
		
		int rand = Random.Range(0, BlockContainerEmpty.MAX_BLOCKS);
		
		for(int z = 0; z < BlockContainerEmpty.MAX_BLOCKS; ++z)
		{
			var type = BlockType.Moveable;
			if(rand == z) type = BlockType.Air;
			column.AddBlock(type);
		}
	}

	private void LoadFile(out float location)
	{
		LoadJson();

		location = 2.5f;
		int colCount = 0;
		if(levelSegments != null)
		{
			foreach(var segId in levelSegments.Level)
			{
				var segment = levelSegments.GetSegmentById(segId);
				if(segment != null)
				{
					AddFillerColumn(location);
					location++;

					foreach(var column in segment.Columns)
					{
						colCount++;
						var clone = Object.Instantiate(blockContainerPrefab);
						clone.name = "Column " + (colCount).ToString();
						clone.transform.SetParent(transform);
						clone.transform.position = new Vector3(location,0,0);
						
						var newColumn = clone.GetComponent<BlockContainerEmpty>();
						columns.Add(newColumn);
						
						foreach(var block in column.FixedBlocks)
						{
							newColumn.AddBlock(block);
						}
						
						location++;
					}
				}
			}

			AddFillerColumn(location);
			location++;
		}
	}

	private void RegisterPrefabs()
	{
		if(!prefabByType.ContainsKey(BlockType.Air)) prefabByType.Add(BlockType.Air,air);
		if(!prefabByType.ContainsKey(BlockType.Moveable)) prefabByType.Add(BlockType.Moveable,moveable);
		if(!prefabByType.ContainsKey(BlockType.Immovable)) prefabByType.Add(BlockType.Immovable,immoveable);
		if(!prefabByType.ContainsKey(BlockType.Spike)) prefabByType.Add(BlockType.Spike,spike);
		if(!prefabByType.ContainsKey(BlockType.Fire)) prefabByType.Add(BlockType.Fire,fire);
		if(!prefabByType.ContainsKey(BlockType.Flower)) prefabByType.Add(BlockType.Flower,flower);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private BlockType[] MakeRandomColumn()
	{
		BlockType[] blocks = new BlockType[BlockContainerEmpty.MAX_BLOCKS];

		int rand = Random.Range(0, BlockContainerEmpty.MAX_BLOCKS);

		for(int i = 0 ; i < BlockContainerEmpty.MAX_BLOCKS; ++i)
		{
			if(i == rand)
			{
				blocks.SetValue(BlockType.Air,i);
			}
			else
			{
				blocks.SetValue(GetRandomBlockType(),i);
			}
		}

		return blocks;
	}

	private BlockType GetRandomBlockType()
	{
		System.Array A = System.Enum.GetValues(typeof(BlockType));
		return (BlockType)A.GetValue(UnityEngine.Random.Range(0,A.Length));
	}

	public void LoadJson()
	{
		TextAsset levelFile = null;
		XmlSerializer serializer = new XmlSerializer(typeof(LevelSegments));
		if(!Application.isEditor)
		{
			levelFile = Resources.Load(LEVEL_FILE) as TextAsset;
			if(levelFile != null)
			{
				using(var reader = new System.IO.StringReader(levelFile.text))
				{
					try
					{
						levelSegments = serializer.Deserialize(reader) as LevelSegments;
					}
					catch(System.Exception e)
					{
						e.GetBaseException();
					}
					reader.Close(); 
				}
			}
		}
		else
		{
			using (StreamReader r = new StreamReader(Application.dataPath+"/Resources/" + LEVEL_FILE + ".xml"))
			{
				try
				{
					levelSegments = serializer.Deserialize(r) as LevelSegments;
				}
				catch(System.Exception e)
				{
					e.GetBaseException();
				}
			}
		}

		if(levelSegments != null)
		{
			foreach(var segment in levelSegments.Segments)
			{
				foreach(var column in segment.Columns)
				{
					column.FixUp();
				}
			}
		}
		if(levelFile != null) Resources.UnloadAsset(levelFile);
	}
}
