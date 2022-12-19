using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem
{
	/// <summary>Types of Save Data.</summary>
	public enum SaveDataTypes
	{
		LevelCleared, 	/// <summary>Level Cleared Save Data.</summary>
		StarsAchieved, 	/// <summary>Stars Achieved Save Data.</summary>
		JewelObtained 	/// <summary>Jewel Obtained Save Data.</summary>
	}

	/// <summary>(Overload Method) Saves data.</summary>
	/// <param name="_dataKey">Data Key ID.</param>
	/// <param name="_saveData">Data that will be saved.</param>
	public static void SaveData(string _dataKey, string _dataValue)
	{
		PlayerPrefs.SetString(_dataKey, _dataValue);
	}

	/// <summary>(Overload Method) Saves data.</summary>
	/// <param name="_dataKey">Data Key ID.</param>
	/// <param name="_saveData">Data that will be saved.</param>
	public static void SaveData(string _dataKey, int _dataValue)
	{
		PlayerPrefs.SetInt(_dataKey, _dataValue);
	}

	/// <summary>(Overload Method) Saves data.</summary>
	/// <param name="_dataKey">Data Key ID.</param>
	/// <param name="_saveData">Data that will be saved.</param>
	public static void SaveData(string _dataKey, bool _dataValue)
	{
		PlayerPrefsX.SetBool(_dataKey, _dataValue);
	}

	/// <summary>(Overload Method) Saves data.</summary>
	/// <param name="_dataKey">Data Key ID.</param>
	/// <param name="_saveData">Data that will be saved.</param>
	public static void SaveData(string _dataKey, string[] _dataValues)
	{
		PlayerPrefsX.SetStringArray(_dataKey, _dataValues);
	}

	/// <summary>(Overload Method) Loads date.</summary>
	/// <param name="_dataKey">Data Key ID.</param>
	/// <returns>Requested data.</returns>
	public static string LoadString(string _dataKey)
	{
		return PlayerPrefs.GetString(_dataKey);
	}

	/// <summary>(Overload Method) Loads date.</summary>
	/// <param name="_dataKey">Data Key ID.</param>
	/// <returns>Requested data.</returns>
	public static int LoadInt(string _dataKey)
	{
		return PlayerPrefs.GetInt(_dataKey);
	}

	/// <summary>(Overload Method) Loads date.</summary>
	/// <param name="_dataKey">Data Key ID.</param>
	/// <returns>Requested data.</returns>
	public static bool LoadBool(string _dataKey)
	{
		return PlayerPrefsX.GetBool(_dataKey);
	}

	/// <summary>(Overload Method) Loads date.</summary>
	/// <param name="_dataKey">Data Key ID.</param>
	/// <returns>Requested data.</returns>
	public static string[] LoadStringArray(string _dataKey)
	{
		return PlayerPrefsX.GetStringArray(_dataKey);
	}

	/// <summary>(Overload Method) Saves datas.</summary>
	/// <param name="_saveDatas">Datas that will be saved.</summary>
	public static void SaveDatas(params SaveDataTypes[] _saveDatas)
	{
		//...
	}
}
