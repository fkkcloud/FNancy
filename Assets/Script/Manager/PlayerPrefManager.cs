using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	// const is also static in class in c#!!
	const string MASTER_VOLUME_KEY = "master_volume";
	const string FPS = "fps";
	const string LEVEL_KEY = "level_unlocked_";

	/// <summary>
	/// Master Volume
	/// </summary>
	public static void SetMasterVolume(float value){
		PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, value);
	}

	public static float GetMasterVolume(){
		return PlayerPrefs.GetInt (MASTER_VOLUME_KEY, 1); // when master volume was not set, 1 will be returned
	}

	/// <summary>
	/// FPS
	/// </summary>
	public static void SetFPS(int fps){
		PlayerPrefs.SetInt (FPS, fps);
	}

	public static int GetFPS(){
		return PlayerPrefs.GetInt (FPS, 60);
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
