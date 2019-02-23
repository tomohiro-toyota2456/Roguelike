using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class UserBoxDatabase : MonoBehaviour
{
	[Serializable]
	public class UserBoxItemData : IUserBoxController
	{
		public string uniqueId;
		public int id;
		[SerializeField]
		private List<InventoryItemData> list;


		public void Add(InventoryItemData data)
		{
			if(list == null)
			{
				list = new List<InventoryItemData>();
			}

			if(!list.Contains(data))
			{
				list.Add(data);
			}
		}

		public void Add(InventoryItemData[] data)
		{
			if (list == null)
			{
				list = new List<InventoryItemData>();
			}

			list.AddRange(data);

		}

		public void Remove(InventoryItemData data)
		{
			if (list == null)
			{
				list = new List<InventoryItemData>();
			}

			list.Remove(data);
		}

		public InventoryItemData[] ToArray()
		{
			if (list == null)
			{
				list = new List<InventoryItemData>();
			}

			return list.ToArray();
		}

		public void SetPossession(int idx,int num,string uId)
		{
			if(list[idx].UniqueId != uId)
			{

				return;
			}
			list[idx].PossessionNum = num;
		}

		public IInventoryItemDataViewer[] GetInventory()
		{
			return list.Select(_ => (IInventoryItemDataViewer)_).ToArray();
		}
	}

	public interface IUserBoxController
	{
		void Add(InventoryItemData data);
		void SetPossession(int idx, int num,string uId);
		IInventoryItemDataViewer[] GetInventory();
	}

	string key = "9fh0@qmcq-j-q";

	[Serializable]
	public class JsonConvertData
	{
		public string uniqueId;
		public int id;
		public InventoryItemData[] list;
	}

	public class JsonArrayData
	{
		public JsonConvertData[] data;
	}

	List<UserBoxItemData> userBoxItemList;

	public string AddData(int id)
	{
		string uniqueId = "";
		//ユニークId生成
		for (int i = 0; i < 100; i++)
		{
			Guid guid = Guid.NewGuid();
			uniqueId = guid.ToString("N");
			if (!CheckUniqueId(uniqueId))
			{
				break;
			}
		}

		UserBoxItemData data = new UserBoxItemData();
		data.uniqueId = uniqueId;
		data.id = id;
		return uniqueId;
	}

	public void RemoveData(string uniqueId)
	{
		var data = SearchData(uniqueId);

		if(data == null)
		{
			return;
		}

		userBoxItemList.Remove(data);
	}

	 UserBoxItemData SearchData(string uniqueId)
	{
		for (int i = 0; i < userBoxItemList.Count; i++)
		{
			var data = userBoxItemList[i];
			if (data.uniqueId.Equals(uniqueId))
			{
				return data;
			}
		}
		return null;
	}

	public IUserBoxController SearchController(string uniqueId)
	{
		for (int i = 0; i < userBoxItemList.Count; i++)
		{
			var data = userBoxItemList[i];
			if (data.uniqueId.Equals(uniqueId))
			{
				return data as IUserBoxController;
			}
		}
		return null;
	}

	bool CheckUniqueId(string uniqueId)
	{
		for (int i = 0; i < userBoxItemList.Count; i++)
		{
			var data = userBoxItemList[i];
			if (data.uniqueId.Equals(uniqueId))
			{
				return true;
			}
		}

		return false;
	}

	public void Save()
	{
		var data = ConvertFromUserBoxItem();
		var array = new JsonArrayData();
		array.data = data;
		string json = JsonUtility.ToJson(array);
		PlayerPrefs.SetString(key, json);
	}

	public void Load()
	{
		string json = PlayerPrefs.GetString(key, "");
		userBoxItemList = new List<UserBoxItemData>();
		if (string.IsNullOrEmpty(json))
		{
			return;
		}

		var array = JsonUtility.FromJson<JsonArrayData>(json);
		userBoxItemList = ConvertFromJsonConvertData(array.data);
	}

	JsonConvertData[] ConvertFromUserBoxItem()
	{
		JsonConvertData[] data = new JsonConvertData[userBoxItemList.Count];
		for(int i = 0; i < userBoxItemList.Count;i++)
		{
			JsonConvertData jData = new JsonConvertData();
			jData.uniqueId = userBoxItemList[i].uniqueId;
			jData.id = userBoxItemList[i].id;
			jData.list = userBoxItemList[i].ToArray();
			data[i] = jData;
		}

		return data;
	}

	List<UserBoxItemData> ConvertFromJsonConvertData(JsonConvertData[] convertData)
	{
		List<UserBoxItemData> list = new List<UserBoxItemData>();

		foreach(var data in convertData)
		{
			UserBoxItemData uData = new UserBoxItemData();
			uData.id = data.id;
			uData.uniqueId = data.uniqueId;
			uData.Add(data.list);
			list.Add(uData);
		}
		return list;
	}

}
