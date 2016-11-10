using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class GameState : MonoBehaviour {

	[Space(10)]
	[Header("General")]
	public LevelManager LevelManager;
	public GameObject ClearText;

	[Space(10)]
	[Header("Character")]
	public GameObject Character;

	[Space(10)]
	[Header("Audio")]
	public GameObject Audio;
	public AudioClip SoundSuccess;
	public AudioClip SoundFail;

	public enum CurrentState {Playing, Loading, Moving, Dead};
	public CurrentState _state;

	private StageManager _stageManager;


	// Use this for initialization
	void Start () {
		Character.GetComponent<Animation> ().Play ("Wait");

		if (!GameObject.FindObjectOfType<MusicManager> ().IsPlaying ()) {
			GameObject.FindObjectOfType<MusicManager> ().Play (Application.loadedLevel);
		}

		_stageManager = GameObject.FindObjectOfType<StageManager> ();
		_stageManager.InitStages ();

		_state = CurrentState.Playing;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnLevelWasLoaded(int level){
		//InitStages ();
	}

	public void PlayerAction(){
		Character.GetComponent<Animation> ().Play ("Attack");

	}

	public void PlayerDead(){
		_state = CurrentState.Dead;
		GameObject.FindObjectOfType<MusicManager> ().Stop ();
		Character.GetComponent<Animation> ().Play ("Dead");
		Audio.GetComponent<AudioSource> ().clip = SoundFail;
		Audio.GetComponent<AudioSource> ().Play ();
		Invoke ("GoToMainMenu", SoundFail.length);
	}

	public void PlayerMove(){
		Character.GetComponent<Animation> ().Play ("Walk");

	}

	public void PlayerIdle(){
		Character.GetComponent<Animation> ().Play ("Wait");
	}

	public void HandleClear()
	{
		ClearText.SetActive (true);
	}

	void GoToMainMenu()
	{
		LevelManager.LoadMainMenu();
	}
}