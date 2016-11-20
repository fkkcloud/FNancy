using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameState : MonoBehaviourHelper {

	[Space(10)]
	[Header("General")]
	public LevelManager LevelManager;
	public GameObject BombFX;
	public GameObject HitFX;
	public GameObject ClearScreen;
	public GameObject ClearText;

	[Space(10)]
	[Header("Audio")]
	public AudioClip SoundPerfect;
	public AudioClip SoundFail;
	public AudioClip SoundClear;
	public AudioClip SoundHit;

	public enum CurrentState {Playing, Loading, Moving, GameOver};
	public CurrentState state = CurrentState.Playing;

	private AudioSource _sfxPlayer;

	void Start(){
		_sfxPlayer = GetComponent<AudioSource> ();

		gameCharacter.Play ("Wait");

		stageManager.InitStages ();

		barAnimator.Setup (stageManager.StageDesignData.stageDatas);

		if (!musicManager.IsPlaying ()) {
			musicManager.Play (Application.loadedLevel);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!stageManager.IsReady || gameState.state == GameState.CurrentState.GameOver)
			return;

		currentStage.gameMode.Tick();

		// touch on screen
		if (Input.GetMouseButtonDown (0)) {
			if (gameState.state == GameState.CurrentState.Moving)
				return;

			gameState.PlayerAction ();

			currentStage.gameMode.Act();
		}
	}

	public void Clear(){
		ClearScreen.SetActive (true);
		LeanTween.alpha (ClearScreen, 1f, 1.1f);
		LeanTween.moveLocalX (ClearText, -0.4f, 0.2f);
	}

	void InitStages(){
		
	}

	void OnLevelWasLoaded(int level){
		//InitStages ();
	}

	public void PlayerAction(){
		gameCharacter.Play ("Attack");
	}

	public void PlaySFX(AudioClip clip, float pitch){
		_sfxPlayer.GetComponent<AudioSource> ().clip = clip;
		_sfxPlayer.GetComponent<AudioSource> ().pitch = pitch;
		_sfxPlayer.GetComponent<AudioSource> ().Play ();
	}
		
	public void PlayHitFX(Vector3 position)
	{
		gameState.HitFX.transform.position = position;
		gameState.HitFX.GetComponent<ParticleSystem> ().Play ();
	}

	public void PlayRegularClearSound()
	{
		PlaySFX (gameState.SoundClear, 1f);
	}

	public void PlayPerfectSound()
	{
		PlaySFX (gameState.SoundPerfect, (gameCharacter.currentPerfect + 1) * 0.8f);
	}

	public void PlayHitSound()
	{
		PlaySFX (gameState.SoundHit, 2f);
	}

	public void GameOver(){

		state = CurrentState.GameOver;

		musicManager.Stop ();

		gameCharacter.Play ("Dead");

		PlaySFX (SoundFail, 1.0f);

		Invoke ("GoToMainMenu", SoundFail.length);

		BombFX.SetActive (true);
		BombFX.GetComponent<ParticleSystem> ().Play ();

		currentStage.HideBomb ();
	}

	public void StageClear(){
		gameCharacter.Play ("Walk");
		stageManager.GoToNextStage ();
		barAnimator.UpdateDotPosition ();
		StartCoroutine (StaticUtils.Shake(0.2f, 0.052f));
	}

	public void PlayerIdle(){
		gameCharacter.Play ("Wait");
	}
		
	void GoToMainMenu()
	{
		LevelManager.LoadMainMenu();
	}
}