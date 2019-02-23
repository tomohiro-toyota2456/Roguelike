using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;
using System;

public class Inventory
{
	List<InventoryItemData> list;
	UserItemDatabase userItemDatabase;
	UserBoxDatabase userBoxDatabase;

	//手持ちに追加
	private void AddData(DropItemData dropData,IInventoryItemDataViewer[] inventory,Action<int,string,int> setPossessionAction,Action<InventoryItemData> addListAction)
	{
		bool enableAddition = false;
		
		if(enableAddition)
		{
			int num = dropData.Num;
			int max = 99;
			int idx = 0;
			foreach (var data in inventory)
			{
				if(data.Id == dropData.Id)
				{
					int space = max - data.PossessionNum;
					int additiveNum = (space - num) >= 0 ? num : space;

					setPossessionAction(userItemDatabase.AddPossessionNum(data.UniqueId, additiveNum), data.UniqueId, idx);
					num -= additiveNum;

					if(num <= 0)
					{
						return;
					}
				}
				idx++;
			}

			if(num > 0)
			{
				//加算しきれずに溢れたので新規枠として追加
				//ドロップデータの書き換えを行う
				DropItemData drop = new DropItemData();
				drop.Set(dropData.Id, dropData.Lv, num, dropData.AvailableNum, dropData.ItemType);
				dropData = drop;
			}
		}

		InventoryItemData invData = new InventoryItemData();
		invData.Id = dropData.Id;
		invData.PossessionNum = dropData.Num;
		invData.AvailableNum = dropData.AvailableNum;
		invData.UniqueId = userItemDatabase.AddData(dropData.Id, dropData.Lv, dropData.Num, dropData.AvailableNum);

		invData.ItemType = dropData.ItemType;
		addListAction(invData);
	}

	public void Add(DropItemData dropData)
	{
		var type = dropData.ItemType;

		switch(type)
		{
			case ItemBaseData.Type.Box:
				DropBoxItemData box = dropData as DropBoxItemData;
				if(box != null)
				{
					var uniqueId = userBoxDatabase.AddData(box.Id);
					var boxController = userBoxDatabase.SearchController(uniqueId);
					var drops = box.GetBoxContents();
					//箱の中身をデータベースと箱に登録
					for (int i = 0; i < drops.Length; i++)
					{
						AddData(drops[i],boxController.GetInventory(), (num, uId, idx) =>
						{
							boxController.SetPossession(idx, num,uniqueId);
						}, (invData) =>
						{
							boxController.Add(invData);
						});
					}

					InventoryItemData inData = new InventoryItemData();
					inData.Id = box.Id;
					inData.UniqueId = uniqueId;
					inData.ItemType = type;
					inData.PossessionNum = 1;
					inData.AvailableNum = -1;
					list.Add(inData);
				}

				break;

			default:
				AddData(dropData, GetInventory(), (num,uId,idx)=>
				{
					var data = list[idx];
					if(data.UniqueId != uId)
					{
						return;
					}

					data.PossessionNum = num;
				},(invData)=>
				{
					list.Add(invData);
				});
				break;
		}
	}

	/// <summary>
	/// 捨てる
	/// </summary>
	public void Dump(int idx)
	{
		var data = list[idx];
		userItemDatabase.RemoveData(data.UniqueId);
	}

	/// <summary>
	/// 使用
	/// </summary>
	public void Use(int idx,int num,bool isZeroRemoving)
	{
		var data = list[idx];
		var viewData = userItemDatabase.UseItem(data.UniqueId, num, isZeroRemoving);
		if(viewData.id !=-1)
		{
			data.PossessionNum = viewData.possessionNum;
			data.AvailableNum = viewData.availableNum;
		}
	}

	/// <summary>
	/// 投げる
	/// </summary>
	public DropItemData ThrowItem(InventoryItemData data)
	{
		DropItemData dropData = new DropItemData();

		var viewData = userItemDatabase.RemoveData(data.UniqueId);
		list.Remove(data);
		dropData.Set(viewData.id, viewData.lv, viewData.possessionNum, viewData.availableNum, viewData.type);

		return dropData;
	}

	public DropBoxItemData ThrowBox(InventoryItemData data)
	{
		DropBoxItemData dropBoxItemData = new DropBoxItemData();
		var box = userBoxDatabase.SearchController(data.UniqueId);
		var inventory = box.GetInventory();
		foreach(var item in inventory)
		{
			DropItemData itemData = new DropItemData();
			var viewData = userItemDatabase.RemoveData(item.UniqueId);
			itemData.Set(viewData.id, viewData.lv, viewData.possessionNum, viewData.availableNum, viewData.type);
			dropBoxItemData.Add(itemData);
		}

		userBoxDatabase.RemoveData(data.UniqueId);
		return dropBoxItemData;
	}



	/// <summary>
	/// インベントリの中身を取得
	/// </summary>
	public IInventoryItemDataViewer[] GetInventory()
	{
		return list.Select(_ => (IInventoryItemDataViewer)_).ToArray();
	}


}
