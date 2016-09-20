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

}

