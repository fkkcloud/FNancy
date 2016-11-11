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

	public float duration;
	public float magnitude;

	IEnumerator Shake() {

		float elapsed = 0.0f;

		Vector3 originalCamPos = Camera.main.transform.position;

		while (elapsed < duration) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			Camera.main.transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

			yield return null;
		}

		Camera.main.transform.position = originalCamPos;
	}

	// Use this for initialization
	void Start () {
		_gameState = GameObject.FindObjectOfType<GameState> ();
		_gameCharacter = GameObject.FindObjectOfType<GameCharacter> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_stages.Length < 1 || _gameState._state == GameState.CurrentState.Dead)
			return;

		if (timerOn) {

			// for default warrior
			if (_gameCharacter._state == GameCharacter.CurrentState.Undefeatable) {
				_gameCharacter.currentUndefeatCount += 1;
				_gameCharacter.UpdateCharacterState ();
				StartCoroutine("Shake");
				StageClear ();
				return;
			}

			// update timer
			timer += (TimerSpeed * Time.deltaTime);
			_stages[_currentStageID].TextMeshTimer.text = timer.ToString("0.0"); // TODO : make it as function for polyomrphism

			CurrentLayerTime = _stages [_currentStageID].GetCurrentLayerTime ();

			// when player does not do anything
			if (timer > CurrentLayerTime + 0.5f) {
				_gameState.PlayerDead ();
			}

			// touch on screen
			if (Input.GetMouseButtonDown (0))
			{
				if (_gameState._state == GameState.CurrentState.Moving)
					return;

				StartCoroutine("Shake");
				_gameState.PlayerAction ();

				/*
				if (_gameCharacter._state == GameCharacter.CurrentState.Undefeatable) {

					_gameCharacter.currentUndefeatCount += 1;
					_gameCharacter.UpdateCharacterState ();

					if (_gameCharacter._state == GameCharacter.CurrentState.Undefeatable)
						_feedMsg.text = "Undefeatable\n" + _gameCharacter.currentUndefeatCount + "/" + _gameCharacter.undefeatMax;
					else
						_feedMsg.text = "Done";

					StageClear ();
					return;
				}
				*/

				if (timer >= CurrentLayerTime - 0.5f && timer <= CurrentLayerTime + 0.5f) {
					
					if (timer >= CurrentLayerTime - 0.2f && timer <= CurrentLayerTime + 0.2f) {
						
						_gameCharacter.currentPerfect += 1;
						_gameCharacter.UpdateCharacterState ();

					} else {
						
						_gameCharacter.StateReset ();

					}

					Evaluate ();
				} else {
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
			_stages [i].StageObj.SetActive (false);

			ZPos += StageLength;
		}

		_currentStageID = 0;

		// init current stages text
		_stages [_currentStageID].StageObj.SetActive (true);
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
		float MoveDuration = 0.9f;
		for (int i = 0; i < _stages.Length; i++) {
			GameObject CurrStage = _stages [i].StageObj;
			float newZPos = CurrStage.transform.position.z - ZPos;
			LeanTween.moveZ (CurrStage, newZPos, MoveDuration).setEase (LeanTweenType.easeInQuad);
		}

		Invoke ("DisablePrevStage", MoveDuration * 2f);
		Invoke ("EnableTimer", MoveDuration * 1.2f);

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
