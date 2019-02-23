using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserMonsterTalismanDatabase : MonoBehaviour
{
	public class UserMonsterTalismanData
	{
		public string uniqueId;
		public int monsterId;
		public int lv;
	}

	public struct Info
	{
		public int monsterId;
		public int lv;
	}

	public class JsonConverData
	{
		public UserMonsterTalismanData[] data;
	}

	string key = "WFfejhfojeofjewov";
	List<UserMonsterTalismanData> userMonsterTalismanList = new List<UserMonsterTalismanData>();

	public string Add(int monsterId,int lv)
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

		UserMonsterTalismanData data = new UserMonsterTalismanData();
		data.monsterId = monsterId;
		data.uniqueId = uniqueId;
		data.lv = lv;
		userMonsterTalismanList.Add(data);
		return uniqueId;
	}

	public void RemoveAll()
	{
		userMonsterTalismanList.Clear();
	}

	public Info Search(string uniqueId)
	{
		Info info;
		info.monsterId = -1;
		info.lv = -1;
		foreach (var data in  userMonsterTalismanList)
		{
			if(data.uniqueId.Equals(uniqueId))
			{
				info.monsterId = data.monsterId;
				info.lv = data.lv;
				return info;
			}
		}
		return info;
	}

	public void Save()
	{
		JsonConverData data = new JsonConverData();
		data.data = userMonsterTalismanList.ToArray();
		string json = JsonUtility.ToJson(data);
		PlayerPrefs.SetString(key, json);
	}

	public void Load()
	{
		string json = PlayerPrefs.GetString(key, "");
		userMonsterTalismanList = new List<UserMonsterTalismanData>();
		if (string.IsNullOrEmpty(json))
		{
			return;
		}

		var data = JsonUtility.FromJson<JsonConverData>(json);
		userMonsterTalismanList.AddRange(data.data);
	}

	
	/// <summary>
	/// ユニークIDが存在しているかチェックする
	/// ある場合はtrue ない場合はfalse
	/// </summary>
	bool CheckUniqueId(string uniqueId)
	{
		for (int i = 0; i < userMonsterTalismanList.Count; i++)
		{
			var data = userMonsterTalismanList[i];
			if (data.uniqueId.Equals(uniqueId))
			{
				return true;
			}
		}

		return false;
	}
}
