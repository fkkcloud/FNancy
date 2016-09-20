using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	// const is also static in class in c#!!
	const string MASTER_VOLUME_KEY = "master_volume";
	const string FPS = "fps";
	const string SOULS = "souls";
	const string SFX_VOLUME_KEY = "sfx_volume";
	const string LEVEL_KEY = "level_unlocked_";

	/// <summary>
	/// Master Volume
	/// </summary>
	public static void SetMasterVolume(int isVolumeOn){
		if (isVolumeOn == 1) {
			PlayerPrefs.SetInt (MASTER_VOLUME_KEY, 1);
		} else {
			PlayerPrefs.SetInt (MASTER_VOLUME_KEY, 0);
		}
	}

	public static int GetMasterVolume(){
		return PlayerPrefs.GetInt (MASTER_VOLUME_KEY);
	}



	/// <summary>
	/// SFX Volume
	/// </summary>
	public static void SetSFXVolume(int isVolumeOn){
		if (isVolumeOn == 1) {
			PlayerPrefs.SetInt (SFX_VOLUME_KEY, 1);
		} else {
			PlayerPrefs.SetInt (SFX_VOLUME_KEY, 0);
		}
	}

	public static int GetSFXVolume(){
		return PlayerPrefs.GetInt (SFX_VOLUME_KEY);
	}

	/// <summary>
	/// FPS
	/// </summary>
	public static void SetFPS(int fps){
		PlayerPrefs.SetInt (FPS, fps);
	}

	public static int GetFPS(){
		return PlayerPrefs.GetInt (FPS);
	}
		

	/*
	/// <summary>
	/// Unlock Level
	/// </summary>
	/// <param name="level">Level.</param>
	public static void UnlockLevel(int level){
		if (level <= Application.loadedLevel - 1) {
			PlayerPrefs.SetInt (LEVEL_KEY + level.ToString(), 1);
		} else {
			Debug.LogError ("Trying to unlock that is not in build order");
		}
	}

	public static bool IsLevelUnlocked(int level){
		if (level <= Application.loadedLevel - 1) {
			int level_value = PlayerPrefs.GetInt (LEVEL_KEY + level.ToString());
			return (level_value == 1);
		} else {
			Debug.LogError ("Trying to ask for unloaded level");
			return false;
		}
	}
	*/

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
