using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxItemData : DropItemData
{
	List<DropItemData> boxList = new List<DropItemData>();
	public void Add(DropItemData dropItem)
	{
		boxList.Add(dropItem);
	}

	public DropItemData[] GetBoxContents()
	{
		return boxList.ToArray();
	}
}
