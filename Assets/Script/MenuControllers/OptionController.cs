using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionController : MonoBehaviour {

	public LevelManager levelManager;

	public string MainMenuSceneName;

	private MusicManager _musicManager;

	public Slider SliderVolume;

	// Use this for initialization
	void Start () {
		_musicManager = GameObject.FindObjectOfType<MusicManager> ();

		// set callback for slider volume changes
		SliderVolume.onValueChanged.AddListener (SetMasterVolume);
		SliderVolume.minValue = 0.0f;
		SliderVolume.minValue = 1.0f;

		InitSound ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void SetDefaults(){
		
	}

	public void SaveAndExit(){
		levelManager.LoadLevel (MainMenuSceneName);
	}

	public void SetMasterVolume(float value){
		PlayerPrefsManager.SetMasterVolume (value);
		_musicManager.SetVolume (value);
	}

	private void InitSound(){
		if (_musicManager) {
			float value = PlayerPrefsManager.GetMasterVolume ();
			_musicManager.SetVolume(value);
			SliderVolume.value = value;
		}
	}

	public void TogglePowerSave(){
		int value;

		if (PlayerPrefsManager.GetFPS () == 30) {
			value = 60;
			Application.targetFrameRate = 60;
		} else {
			value = 30;
			Application.targetFrameRate = 30;
		}

		PlayerPrefsManager.SetFPS (value);
	}

	// callback for master volume btn
	public void ToggleMasterVolume(){
		int value;

		// reverse the result
		if (PlayerPrefsManager.GetMasterVolume () == 1) {
			value = 0;
			_musicManager.SetVolume(0);
		} else {
			value = 1;
			_musicManager.SetVolume(1);
		}

		PlayerPrefsManager.SetMasterVolume (value);
	}
}


