using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public float autoLoadNextLevelDuration;
	public bool autoLoadEnable;

	void Start(){
		if (autoLoadEnable) {
			// Splash Screen -> Main Menu
			Invoke ("LoadNextLevel", autoLoadNextLevelDuration);
		}
	}

	public void LoadLevel(string name){
		Debug.Log ("New Level load requested: " + name);
		Application.LoadLevel (name);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}

	public void LoadNextLevel(){
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void LoadMainMenu(){
		Application.LoadLevel ("01_000_MainMenu");
	}

	public void LoadGameLevel(){
		Application.LoadLevel ("02_000_Base");
	}

	public void LoadLevelRoom(){
		Application.LoadLevel ("01_002_LevelRoom");
	}

	public void LoadStoreRoom(){
		Application.LoadLevel ("01_003_Store");
	}
}
	