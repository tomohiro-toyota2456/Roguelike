using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserItemDatabase : MonoBehaviour
{
	[System.Serializable]
	public class UserItemData
	{
		public string uniqueId;
		public int id;
		public int lv;
		public int availableNum;
		public int possessionNum;
		public ItemBaseData.Type type;
	}

	public struct ViewData
	{
		public int id;
		public int lv;
		public int availableNum;
		public int possessionNum;
		public ItemBaseData.Type type;
	}

	List<UserItemData> userItemList;

	[System.Serializable]
	public class JsonConvertData
	{
		public UserItemData[] data;
	}

	readonly string JsonKey = "3rmfj9aJF90fjaff";

	/// <summary>
	/// データ追加　return:ユニークId
	/// 使用可能回数が存在する場合は1以上の値をいれる　存在しない場合は-1を入れる
	/// </summary>
	public string AddData(int id,int lv,int possessionNum,int avaliableNum = -1)
	{
		string uniqueId ="";
		//ユニークId生成
		for(int i = 0; i < 100; i++)
		{
			Guid guid = Guid.NewGuid();
			uniqueId = guid.ToString("N");
			if (!CheckUniqueId(uniqueId))
			{
				break;
			}
		}
		//データ生成
		UserItemData userItemData = new UserItemData();
		userItemData.uniqueId = uniqueId;
		userItemData.id = id;
		userItemData.possessionNum = possessionNum;
		userItemData.availableNum = avaliableNum;
		userItemList.Add(userItemData);
		return uniqueId;
	}

	public ViewData RemoveData(string uniqueId)
	{
		ViewData viewData;
		viewData.id = -1;
		viewData.lv = -1;
		viewData.type = ItemBaseData.Type.Item;
		viewData.possessionNum = -1;
		viewData.availableNum = -1;
		for (int i = 0; i < userItemList.Count; i++)
		{
			var data = userItemList[i];
			if (data.uniqueId.Equals(uniqueId))
			{
				userItemList.RemoveAt(i);

				viewData.id = userItemList[i].id;
				viewData.lv = userItemList[i].lv;
				viewData.availableNum = userItemList[i].availableNum;
				viewData.possessionNum = userItemList[i].possessionNum;
				viewData.type = userItemList[i].type;

				return viewData;
			}
		}
		return viewData;
	}

	public int AddPossessionNum(string uniqueId,int num)
	{
		var data = SearchData(uniqueId);
		if (data == null)
			return -1;

		data.possessionNum = data.possessionNum + num;
		return data.possessionNum;
	}

	/// <summary>
	/// 使用報告　isZeroRemovingは使用回数が0以下の場合にリストから削除するかどうか
	/// </summary>
	public ViewData UseItem(string uniqueId,int num = 1,bool isZeroRemoving = false)
	{
		ViewData viewData;
		viewData.id = -1;
		viewData.lv = -1;
		viewData.availableNum = -1;
		viewData.possessionNum = -1;
		viewData.type = ItemBaseData.Type.Item;

		var data = SearchData(uniqueId);
		if(data == null)
		{
			return viewData;
		}

		//閲覧用データに変動しないものを先にセット
		viewData.id = data.id;
		viewData.lv = data.lv;

		//使用可能回数が存在する場合は優先して消費
		if (data.availableNum > 0)
		{
			data.availableNum -= num;
			data.availableNum = data.availableNum < 0 ? 0 : data.availableNum;

			viewData.possessionNum = data.possessionNum;
			viewData.availableNum = data.availableNum;

			num = 0;//使用回数で処理を行った場合はここで０にして所持数を減らさないようにする
		}

		if (data.availableNum == 0)
		{
			//使用可能回数が０で消す場合はここで消える
			if(isZeroRemoving)
			{
				userItemList.Remove(data);
			}
		}

		if(data.possessionNum >0)
		{
			data.possessionNum -= num;

			viewData.possessionNum = data.possessionNum;
			viewData.availableNum = data.availableNum;

			if(data.possessionNum <= 0)
			{
				userItemList.Remove(data);
			}
		}

		return viewData;
	}

	public void Load()
	{
		userItemList = new List<UserItemData>();
		var json = PlayerPrefs.GetString(JsonKey, "");
		if(string.IsNullOrEmpty(json))
		{
			return;
		}
		var convertData = JsonUtility.FromJson<JsonConvertData>(json);
		userItemList.AddRange(convertData.data);
	}

	public void Save()
	{
		JsonConvertData convertData = new JsonConvertData();
		convertData.data = userItemList.ToArray();
		var json = JsonUtility.ToJson(convertData);
		PlayerPrefs.SetString(JsonKey, json);
	}

	/// <summary>
	/// ユニークIDが存在しているかチェックする
	/// ある場合はtrue ない場合はfalse
	/// </summary>
	bool CheckUniqueId(string uniqueId)
	{
		for(int i = 0; i < userItemList.Count; i++)
		{
			var data = userItemList[i];
			if(data.uniqueId.Equals(uniqueId))
			{
				return true;
			}
		}

		return false;
	}

	UserItemData SearchData(string uniqueId)
	{
		for(int i = 0; i < userItemList.Count; i++)
		{
			var data = userItemList[i];
			if(data.uniqueId.Equals(uniqueId))
			{
				return data;
			}
		}

		return null;
	}

	public ViewData Search(string uniqueId)
	{
		ViewData viewData;
		viewData.availableNum = -1;
		viewData.possessionNum = -1;
		viewData.id = -1;
		viewData.lv = -1;
		viewData.type = ItemBaseData.Type.Item;
		for (int i = 0; i < userItemList.Count; i++)
		{
			var data = userItemList[i];
			if (data.uniqueId.Equals(uniqueId))
			{
				viewData.id = data.id;
				viewData.availableNum = data.availableNum;
				viewData.possessionNum = data.possessionNum;
				viewData.lv = data.lv;
				viewData.type = data.type;
				return viewData;
			}
		}

		return viewData;
	}

}
