using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所持アイテム
/// </summary>
[System.Serializable]
public class InventoryItemData : IInventoryItemDataViewer
{ 
	[SerializeField]
	string uniqueId;
	[SerializeField]
	int id;
	[SerializeField]
	int possessionNum;
	[SerializeField]
	int availableNum;
	[SerializeField]
	ItemBaseData.Type type;
	public string UniqueId { get { return uniqueId; } set { uniqueId = value; } }
	public int Id { get { return id; } set { id = value; } }
	public ItemBaseData.Type ItemType { get { return type; } set { type = value; } }
	public int PossessionNum { get { return possessionNum; } set { possessionNum = value; } }
	public int AvailableNum { get { return availableNum; } set { availableNum = value; } }
}

public interface IInventoryItemDataViewer
{
	string UniqueId { get; }
	int Id { get; }
	int PossessionNum { get; }
	int AvailableNum { get; }
	ItemBaseData.Type ItemType { get; }
}
