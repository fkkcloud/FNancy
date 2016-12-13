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
	public GameObject BarInfo;
	public GameObject GameOverPage;
	public GameObject FeedbackText;

	[Space(10)]
	[Header("Audio")]
	public AudioClip SoundPerfect;
	public AudioClip SoundFail;
	public AudioClip SoundClear;
	public AudioClip SoundHit;

	public enum CurrentState {Playing, Loading, Moving, GameOver};
	public CurrentState state = CurrentState.Playing;

	public enum FeedbackType {Perfect, Good};

	private AudioSource _sfxPlayer;

	void Start(){
		_sfxPlayer = GetComponent<AudioSource> ();

		StartGame ();

	}

	public void StartGame(){
		gameCharacter.Play ("Wait");

		stageManager.InitStages ();

		if (!musicManager.IsPlaying ()) {
			musicManager.Play (Application.loadedLevel);
		}

		//barAnimator.Setup (stageManager.stageDocuments[stageManager.selectedStageDocumentID].StageDesignData.stageDatas);
	}

	public void RestartGame(){
		stageManager.DestroyStages();

		GameOverPage.SetActive (false);

		gameCharacter.Activate ();

		StartGame ();
	}

	// Update is called once per frame
	void Update () {

		// regular game tick
		if (stageManager.IsReady && gameState.state != GameState.CurrentState.GameOver) {
			
			currentStage.gameMode.Tick ();

			// touch on screen
			if (Input.GetMouseButtonDown (0)) {
				if (gameState.state == GameState.CurrentState.Moving)
					return;

				gameState.PlayerAction ();

				// each gamemode's update call
				currentStage.gameMode.ReactOnTouch ();
			}
		} else if (gameState.state == GameState.CurrentState.GameOver) {
			BarInfo.SetActive (true);
		}
	}

	public void Clear(){
		ClearScreen.SetActive (true);
		LeanTween.alpha (ClearScreen, 1f, 1.1f);
		LeanTween.moveLocalX (ClearText, -0.4f, 0.2f);
	}

	void OnLevelWasLoaded(int level){
		//InitStages ();
	}

	public void PlayerAction(){
		gameCharacter.Play ("Attack");
	}

	public void PlayTextFeedBack (GameState.FeedbackType feedbacktype)
	{
		string word = "";
		if (feedbacktype == GameState.FeedbackType.Perfect) {
			word = "PERFECT";
		}
		FeedbackText.GetComponent<Text> ().text = word;
		StartCoroutine (StaticUtils.AnimateFeedBackText(FeedbackText.GetComponent<Text>().rectTransform, 0.3f, 250f, -500f, 100f));
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

		if (state == CurrentState.GameOver)
			return;
		
		state = CurrentState.GameOver;

		musicManager.Stop ();

		gameCharacter.Play ("Dead");


		PlaySFX (SoundFail, 1.0f);

		//Invoke ("GoToMainMenu", SoundFail.length);
		Invoke("ShowGameOverPage", SoundFail.length);

		// bomb explosion
		BombExplosionForGameOver();
	}

	void BombExplosionForGameOver(){
		BombFX.SetActive (true);
		BombFX.GetComponent<ParticleSystem> ().Play ();
		currentStage.HideBomb ();
	}

	void ShowGameOverPage(){
		GameOverPage.SetActive (true);
	}

	public void StageClear(){
		gameCharacter.Play ("Walk");
		stageManager.GoToNextStage ();
		//barAnimator.UpdateDotPosition ();
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