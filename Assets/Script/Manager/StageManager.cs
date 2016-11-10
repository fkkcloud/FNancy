using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

	[Space(10)]
	[Header("Stage")]
	public StageDesigner StageDesignData;
	public GameObject StageObject;
	public float StageLength = 4.5f;
	public float TimerSpeed = 0.8f;

	private float timer = 0f;
	private bool timerOn = true;

	private float CurrentLayerTime;

	private int _currentStageID;
	private Stage[] _stages;
	private GameObject _prevStage;

	private GameState _gameState;
	private GameCharacter _gameCharacter;
	private TextMesh _feedMsg;

	// Use this for initialization
	void Start () {
		_gameState = GameObject.FindObjectOfType<GameState> ();
		_gameCharacter = GameObject.FindObjectOfType<GameCharacter> ();
		_feedMsg = GameObject.FindWithTag ("TimerFeed").GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_stages.Length < 1 || _gameState._state == GameState.CurrentState.Dead)
			return;

		if (timerOn) {
			timer += (TimerSpeed * Time.deltaTime);

			CurrentLayerTime = _stages [_currentStageID].GetCurrentLayerTime ();

			// when player does not do anything
			if (timer > CurrentLayerTime + 0.5f) {
				_feedMsg.text = "GameOver";
				_gameState.PlayerDead ();
			}

			// update timer
			_stages[_currentStageID].TextMeshTimer.text = timer.ToString("0.0"); // TODO : make it as function for polyomrphism


			// touch on screen
			if (Input.GetMouseButtonDown (0))
			{
				if (_gameState._state == GameState.CurrentState.Moving)
					return;

				Debug.Log (_gameCharacter._state);

				_gameState.PlayerAction ();

				if (_gameCharacter._state == GameCharacter.CurrentState.Undefeatable) {
					_gameCharacter.currentPostProd += 1;
					_gameCharacter.UpdateCharacterState ();
					StageClear ();

					if (_gameCharacter._state == GameCharacter.CurrentState.Undefeatable)
						_feedMsg.text = "Undefeatable\n" + _gameCharacter.currentPostProd + "/" + (_gameCharacter.Postprod-1);
					else
						_feedMsg.text = "Done";
					return;
				}

				if (timer >= CurrentLayerTime - 0.5f && timer <= CurrentLayerTime + 0.5f) {
					
					if (timer >= CurrentLayerTime - 0.2f && timer <= CurrentLayerTime + 0.2f) {
						_gameCharacter.currentPreProd += 1;
						_gameCharacter.UpdateCharacterState ();
						_feedMsg.text = "PERFECT\n" + (_gameCharacter.Preprod - _gameCharacter.currentPreProd + 1) + " more!";
					} else {
						_gameCharacter.currentPreProd = 0;
						_gameCharacter.UpdateCharacterState ();
						_feedMsg.text = "Good";
					}
					Evaluate ();
				} else {
					_feedMsg.text = "GameOver";
					_gameState.PlayerDead ();
				}
			}
		}  
	}

	private void StageClear(){
		_gameState.Audio.GetComponent<AudioSource> ().clip = _gameState.SoundSuccess;
		_gameState.Audio.GetComponent<AudioSource> ().pitch = (CurrentLayerTime + 1) * 1.1f;
		_gameState.Audio.GetComponent<AudioSource> ().Play ();

		_gameState.PlayerMove ();
		GoToNextStage ();
	}

	private void StageProceed(){
		_gameState.Audio.GetComponent<AudioSource> ().clip = _gameState.SoundSuccess;
		_gameState.Audio.GetComponent<AudioSource> ().pitch = (CurrentLayerTime + 1) * 1.1f;
		_gameState.Audio.GetComponent<AudioSource> ().Play ();

		_stages[_currentStageID].CurrentLayerID += 1;
		_stages [_currentStageID].UpdateIndicator ();
	}

	private void Evaluate(){
		if (_stages[_currentStageID].IsClear()) {

			StageClear ();

		} else {

			StageProceed ();

		}
	}

	public void InitStages(){
		
		_stages = new Stage[StageDesignData.stageDatas.Length];


		// create all the stages : TODO : create the stages to be optimized!
		float ZPos = 0.0f;
		for (int i = 0; i < StageDesignData.stageDatas.Length; i++) 
		{
			Vector3 pos = new Vector3 (0f, 0f, ZPos);

			_stages [i] = new Stage ();
			_stages [i].InitStageObj (ref StageObject, pos);
			_stages [i].SetStageData (StageDesignData.stageDatas [i]);

			ZPos += StageLength;
		}

		_currentStageID = 0;

		// init current stages text
		_stages [_currentStageID].InitText ();
	}

	void GoToNextStage(){

		if (_stages.Length - 1 == _currentStageID) {
			_gameState.HandleClear ();
			return;
		}

		_gameState._state = GameState.CurrentState.Moving;

		// disable prev stage inactive
		if (_currentStageID - 1 > -1) {
			_prevStage = _stages [_currentStageID - 1].StageObj;
		}

		// animate current stage 
		Animator Anim = _stages[_currentStageID].StageObj.GetComponent<Animator> ();
		Anim.SetTrigger ("BombDownTrigger");
		_stages [_currentStageID].HideTexts ();

		// update current stage to next
		_currentStageID += 1;

		// activate next stage
		_stages [_currentStageID].StageObj.SetActive (true);

		// animate next stage
		Anim = _stages[_currentStageID].StageObj.GetComponent<Animator> ();
		Anim.SetTrigger ("ActivateTrigger");

		// move all the stages
		float ZPos = StageLength;
		float MoveSpeed = 1.0f;
		for (int i = 0; i < _stages.Length; i++) {
			GameObject CurrStage = _stages [i].StageObj;
			float newZPos = CurrStage.transform.position.z - ZPos;
			LeanTween.moveZ (CurrStage, newZPos, MoveSpeed).setEase (LeanTweenType.easeInQuad);
		}

		Invoke ("DisablePrevStage", MoveSpeed * 2f);
		Invoke ("EnableTimer", MoveSpeed * 0.96f);

		timer = 0f;
		timerOn = false;

		// init current stages text
		_stages [_currentStageID].InitText ();
	}

	void DisablePrevStage()
	{
		if (_prevStage)
			_prevStage.SetActive (false);
	}

	void EnableTimer()
	{
		timerOn = true;
		_gameState.PlayerIdle ();
		_gameState._state = GameState.CurrentState.Playing;
	}
}
