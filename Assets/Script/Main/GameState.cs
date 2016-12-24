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
	public GameObject MenuScreen;
	public GameObject MenuButtons;
	public GameObject ClearText;
	public GameObject BarInfo;
	public GameObject GameOverPage;
	public GameObject FeedbackText;
	public GameObject Character;
	public GameObject ClearUI;
	public GameObject ClearMenuButtons;

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

	private GameObject _initialStage;

	void Start(){
		_sfxPlayer = GetComponent<AudioSource> ();

		// main menu bg
		if (stageManager)
			_initialStage = Instantiate (stageManager.StageObject);

		// skip the menu part if its started from level menu
		if (globalVariables.StartedFromLevelRoom ||  globalVariables.Restarted) {
			StartGame ();
		} else {
			MenuScreen.SetActive (true);
			MenuButtons.SetActive (true);
		}
	}

	public void StartGame(){
		_sfxPlayer.Play ();

		gameState.state = GameState.CurrentState.Playing;

		if (!gameCharacter)
			Instantiate (Character);
		
		gameCharacter.Activate ();

		float delaytime = 0.5f;
		stageManager.InitStages (delaytime); // with delaytime
		_initialStage.SetActive (false);

		// UI related controls
		if (globalVariables.StartedFromLevelRoom || globalVariables.Restarted) {
			TurnOffMenuScreen ();
			globalVariables.Restarted = false;
			globalVariables.StartedFromLevelRoom = false;
			gameCharacter.Play ("Wait");
		} else {
			Invoke ("TurnOffMenuScreen", 0.5f);
			LeanTween.alpha (MenuScreen, 0f, 0.2f);
			LeanTween.moveLocalY(MenuButtons, -132f, 0.4f).setEase(LeanTweenType.easeInQuad);
		}

		// animated character to the stage!
		Vector3 originalPos = gameCharacter.gameObject.transform.position;
		gameCharacter.gameObject.transform.position = new Vector3 (originalPos.x, originalPos.y, -5.2f);
		LeanTween.moveLocalZ (gameCharacter.gameObject, originalPos.z, 0.5f).setEase(LeanTweenType.easeOutQuad);
		gameCharacter.Play ("Walk");

		if (!musicManager.IsPlaying ()) {
			musicManager.Play (Application.loadedLevel);
		}

		//barAnimator.Setup (stageManager.stageDocuments[stageManager.selectedStageDocumentID].StageDesignData.stageDatas);
	}

	public void RestartGame(){

		ResetSFX ();
		ResetClearScreen ();
		ResetGameOverScreen ();

		globalVariables.Restarted = true;

		// destroy all level info
		DestroyImmediate (gameCharacter.gameObject);
		stageManager.DestroyStages();

		if (musicManager.IsPlaying ()) {
			musicManager.Stop ();
			musicManager.Play (Application.loadedLevel);
		}

		StartGame ();
	}

	// Update is called once per frame
	void Update () {

		// regular game tick
		if (stageManager && stageManager.IsReady && gameState.state != GameState.CurrentState.GameOver) {
			
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

	void TurnOffMenuScreen(){
		MenuScreen.SetActive (false);
		MenuButtons.SetActive (false);
		gameCharacter.Play ("Wait");
	}

	public void ResetClearScreen(){
		ClearScreen.SetActive (false);
		ClearUI.SetActive (false);
	}

	public void ShowClearScreen(){
		ClearScreen.SetActive (true);
		LeanTween.alpha (ClearScreen, 1f, 1.1f);
		LeanTween.moveLocalY(ClearMenuButtons, -2f, 0.4f).setEase(LeanTweenType.easeOutQuad);
		//StartCoroutine (StaticUtils.AnimateFeedBackText(ClearText.GetComponent<Text>().rectTransform, 0.3f, 215f, -71f, 100f));
		ClearUI.SetActive (true);
	}

	public void ResetGameOverScreen(){
		GameOverPage.SetActive (false);
	}

	void ShowGameOverScreen(){
		GameOverPage.SetActive (true);
		BarInfo.GetComponent<BarAnimator> ().Setup (globalVariables.stageDocuments[globalVariables.SelectedLevel].StageDesignData.stageDatas);
	}


	public void GotoNextLevel()
	{
		//LeanTween.alpha (ClearScreen, 0f, 0.3f);
		LeanTween.moveLocalY(ClearMenuButtons, -132f, 0.4f).setEase(LeanTweenType.easeInQuad);
		globalVariables.SelectedLevel++;
		Invoke ("RestartGame", 0.4f);
	}
		
	public void GameOver(){

		if (state == CurrentState.GameOver)
			return;

		state = CurrentState.GameOver;

		currentStage.Deactivate ();

		musicManager.Stop ();

		gameCharacter.Play ("Dead");
		gameCharacter.Deactivate ();

		PlaySFX (SoundFail, 1.0f);
		Invoke("ShowGameOverScreen", SoundFail.length * 0.475f);

		// bomb explosion
		BombExplosionForGameOver();
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
		StartCoroutine (StaticUtils.AnimateFeedBackText(FeedbackText.GetComponent<Text>().rectTransform, 0.4f, 250f, -500f, 100f));
	}

	public void ResetSFX(){
		_sfxPlayer.GetComponent<AudioSource> ().clip = null;
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

	void BombExplosionForGameOver(){
		BombFX.SetActive (true);
		BombFX.GetComponent<ParticleSystem> ().Play ();
		currentStage.HideCurrentBombOrCage ();
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
		
	public void GoToMainMenu()
	{
		if (musicManager.IsPlaying ()) {
			musicManager.Stop ();
		}
		LevelManager.LoadMainMenu();
	}

	public void GoToLevelRoom()
	{
		if (musicManager.IsPlaying ()) {
			musicManager.Stop ();
		}
		LevelManager.LoadLevelRoom();
	}

	public void GoToStore()
	{
		LevelManager.LoadStoreRoom ();
	}
}