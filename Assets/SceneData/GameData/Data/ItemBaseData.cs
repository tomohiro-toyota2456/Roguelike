using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBaseData : ScriptableObject,IItemBaseDataViewer
{
	public enum Type
	{
		Wepon,
		Armor,
		Item,
		Box,//保管箱
		Talisman,//札
	}
	[SerializeField]
	int id;
	[SerializeField]
	Type itemType;
	[SerializeField]
	bool enableAddtion;
	[SerializeField]
	bool enableStorageAddition;//倉庫加算可能か
	[SerializeField]
	int maxStack = 99;
	[SerializeField]
	int sellingPrice;

	public int Id { get { return id; } set { id = value; } }
	public Type ItemType { get { return itemType; }set { itemType = value; } }
	public bool EnableAddtion { get { return enableAddtion; } set { enableAddtion = value; } }
	public bool EnableStorageAddition { get { return enableStorageAddition; } set { enableStorageAddition = value; } }
	public int MaxStack { get { return maxStack; } set { maxStack = value; } }
	public int SellingPrice { get { return sellingPrice; } set { sellingPrice = value; } }
}

public interface IItemBaseDataViewer
{
	int Id { get; }
	ItemBaseData.Type ItemType { get; }
	bool EnableAddtion { get; }
	int MaxStack { get; }
}
