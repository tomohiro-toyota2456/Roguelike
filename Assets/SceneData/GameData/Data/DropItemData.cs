using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemData
{
	int id;
	int lv;
	int num;
	int availableNum = -1;
	ItemBaseData.Type type;

	public void Set(int id, int lv, int num, int availableNum,ItemBaseData.Type type)
	{
		this.id = id;
		this.lv = lv;
		this.num = num;
		this.availableNum = availableNum;
		this.type = type;
	}

	public int Id { get { return id; } }
	public int Lv { get { return lv; } }
	public int Num { get { return num; } }
	public int AvailableNum { get { return availableNum; } }
	public ItemBaseData.Type ItemType { get { return type; } }

}
