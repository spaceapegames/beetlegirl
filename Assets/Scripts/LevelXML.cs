using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable()]
public class LevelSegments
{
	private List<string> levelOrder = new List<string>();
	public List<string> Level
	{
		get { return levelOrder; }
		set { levelOrder = value; }
	}

	private List<Segment> segment = new List<Segment>();
	public List<Segment> Segments
	{
		get { return segment; }
		set { segment = value; }
	}

	public Segment GetSegmentById(string id)
	{
		if(id.ToLower() == "random")
		{
			return GetRandomSegment();
		}

		foreach(var seg in Segments)
		{
			if(seg.ID.ToLower() == id.ToLower())
			{
				return seg;
			}
		}
		return null;
	}

	public Segment GetRandomSegment()
	{
		int rand = UnityEngine.Random.Range(0, Segments.Count);
		return Segments[rand];
	}
}

[Serializable()]
public class Segment
{
	private string segmentId;
	public string ID
	{
		get { return segmentId; }
		set { segmentId = value; }
	}

	private List<Column> columns = new List<Column>();
	public List<Column> Columns
	{
		get { return columns; }
		set { columns = value; }
	}
}

[Serializable()]
public class Column
{
	private List<string> blocks = new List<string>();
	public List<string> Blocks
	{
		get { return blocks; }
		set { blocks = value; }
	}

	public List<BlockType> FixedBlocks = new List<BlockType>();

	public void FixUp()
	{
		if(FixedBlocks.Count > 0)
		{
			return;
		}

		var immovable = false;
		foreach(var block in blocks)
		{
			var type = GetBlockType(block);
			if(type == BlockType.Immovable || type == BlockType.Fire)
			{
				immovable = true;
			}
		}

		foreach(var block in blocks)
		{
			var type = GetBlockType(block);
			if(type == BlockType.Moveable && immovable)
			{
				type = BlockType.Immovable;
			}
			FixedBlocks.Add(type);
		}
	}

	public static BlockType GetBlockType(string type)
	{
		BlockType parsedEnum = BlockType.Air;
		try
		{
			parsedEnum = (BlockType)System.Enum.Parse(typeof(BlockType),type);
		}
		catch(Exception e)
		{
			
		}
		return parsedEnum;
	}
}