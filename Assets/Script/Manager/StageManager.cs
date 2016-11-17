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

	private GameObject _bombObj;
	private GameObject _perfectText;
	public GameObject StageBar;

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

	private bool IsBombShakable = true;

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

	IEnumerator ShowPerfectUI(){
		_perfectText.transform.position = new Vector3 (4f, _perfectText.transform.position.y, _perfectText.transform.position.z);
		LeanTween.moveLocalX (_perfectText, -5.5f, 0.38f);
		yield return null;
	}

	IEnumerator ShakeBomb() {

		float elapsed = 0.0f;

		Vector3 bombOriginalPos = _bombObj.transform.position;

		while (elapsed < 4f) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / 4f;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= 20f * damper;
			y *= 20f * damper;

			_bombObj.transform.position = new Vector3(bombOriginalPos.x + x, bombOriginalPos.y + y, bombOriginalPos.z);

			yield return null;
		}

		//_bombObj.transform.position = bombOriginalPos;

		//IsBombShakable = true;
	}
		
	// Use this for initialization
	void Start () {
		_gameState = GameObject.FindObjectOfType<GameState> ();
		_gameCharacter = GameObject.FindObjectOfType<GameCharacter> ();

	}

	void ResetBombShakable(){
		IsBombShakable = true;
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
				PlayPerfectSound (); 
				StageClear ();
				return;
			}

			// update timer
			timer += (TimerSpeed * Time.deltaTime);
			_stages[_currentStageID].TextMeshTimer.text = timer.ToString("0.0"); // TODO : make it as function for polyomrphism

			// coloring the bomb
			if (_stages [_currentStageID]._stageData.gamemode == 0) {
				CurrentLayerTime = _stages [_currentStageID].GetCurrentLayerTime ();
				foreach (Renderer r in _bombObj.GetComponentsInChildren<Renderer>()) {
					Color c = r.material.GetColor ("_Color");
					r.material.SetColor ("_Color", new Color (c.r + 0.014f, c.g, c.b));

				}	
			} else if (_stages [_currentStageID]._stageData.gamemode == 1) {
				CurrentLayerTime = _stages [_currentStageID].GetCurrentLayerTime ();
				foreach (Renderer r in _bombObj.GetComponentsInChildren<Renderer>()) {
					Color c = r.material.GetColor ("_Color");
					r.material.SetColor ("_Color", new Color (c.r, c.g, c.b + 0.02f));

				}
			}

			// when player does not do anything
			if (timer > CurrentLayerTime + 0.05f) {
				_gameState.PlayerDead ();
			}



			// coloring the text
			if (timer < CurrentLayerTime - 0.25f) {
				_stages [_currentStageID].TextMeshTimer.color = new Color (0.9f, 0.2f, 0.2f);
			} else if (timer >= CurrentLayerTime - 0.25f && timer <= CurrentLayerTime - 0.1f) {
				_stages [_currentStageID].TextMeshTimer.color = new Color (0.9f, 0.9f, 0.2f);
			} else if (timer > CurrentLayerTime + 0.05f) {
				_stages [_currentStageID].TextMeshTimer.color = new Color (0.9f, 0.2f, 0.2f);
			} else {
				_stages [_currentStageID].TextMeshTimer.color = new Color (0.2f, 0.9f, 0.2f);
			}
			


			if (timer > CurrentLayerTime - 0.28f && IsBombShakable) {
				IsBombShakable = false;
				Animator Anim = _stages[_currentStageID].StageObj.GetComponent<Animator> ();
				Anim.SetTrigger ("BombShakeTrigger");
				Invoke ("ResetBombShakable", 1f);
			}

			// touch on screen
			if (Input.GetMouseButtonDown (0)) {
				if (_gameState._state == GameState.CurrentState.Moving)
					return;

				StartCoroutine ("Shake");
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
				if (_stages [_currentStageID]._stageData.gamemode == 0){
					if (timer >= CurrentLayerTime - 0.25f && timer < CurrentLayerTime + 0.05f) {
						
						if (timer >= CurrentLayerTime - 0.1f && timer < CurrentLayerTime + 0.05f) {
							// CASE PERFECT

							StartCoroutine ("ShowPerfectUI");

							PlayPerfectSound ();
							_gameCharacter.currentPerfect += 1;
							_gameCharacter.UpdateCharacterState ();

						} else {
							_gameState.PlayRegularClearSound ();
							_gameCharacter.StateReset ();

						}

						Evaluate ();
					} else {
						_gameState.PlayerDead ();
					}
				} else if (_stages [_currentStageID]._stageData.gamemode == 1) {
					if (_stages [_currentStageID]._stageData.hp > _stages [_currentStageID].CurrentHP) {
						_stages [_currentStageID].CurrentHP++;
						PlayHitSound ();
						PlayHitFX ();
					} else {
						StartCoroutine("Shake");
						StageClear ();
					}  
				}
			}
		}  
	}

	private void PlayPerfectSound()
	{
		_gameState.Audio.GetComponent<AudioSource> ().clip = _gameState.SoundPerfect;
		_gameState.Audio.GetComponent<AudioSource> ().pitch = (_gameCharacter.currentPerfect + 1) * 0.8f;
		_gameState.Audio.GetComponent<AudioSource> ().Play ();
	}

	private void PlayHitSound()
	{
		_gameState.Audio.GetComponent<AudioSource> ().clip = _gameState.SoundHit;
		_gameState.Audio.GetComponent<AudioSource> ().pitch = 2f;
		_gameState.Audio.GetComponent<AudioSource> ().Play ();
	}

	private void PlayHitFX()
	{
		_gameState.HitFX.GetComponent<ParticleSystem> ().Play ();
	}

	private void StageClear(){

		_gameState.PlayerMove ();
		GoToNextStage ();
	}

	private void StageProceed(){


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

	public void HideBomb(){
		_bombObj.SetActive (false);
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

		StageBar.GetComponent<BarAnimator> ().Setup (StageDesignData.stageDatas);

		_currentStageID = 0;

		// init current stages text
		_stages [_currentStageID].StageObj.SetActive (true);
		_stages [_currentStageID].InitText ();


		foreach(Transform child in _stages[_currentStageID].StageObj.transform){
			if(child.gameObject.tag == "BombObject"){
				_bombObj = child.gameObject;
				break;
			}
		}

		foreach(Transform child in _stages[_currentStageID].StageObj.transform){
			if(child.gameObject.tag == "PerfectText"){
				_perfectText = child.gameObject;
				break;
			}
		}
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

		Invoke ("DisablePrevStage", MoveDuration * 1f);
		Invoke ("EnableTimer", MoveDuration * 1.025f);

		timer = 0f;
		timerOn = false;

		// init current stages text
		_stages [_currentStageID].InitText ();
		foreach(Transform child in _stages[_currentStageID].StageObj.transform){
			if(child.gameObject.tag == "BombObject"){
				_bombObj = child.gameObject;
				break;
			}
		}

		foreach(Transform child in _stages[_currentStageID].StageObj.transform){
			if(child.gameObject.tag == "PerfectText"){
				_perfectText = child.gameObject;
				break;
			}
		}

		StageBar.GetComponent<BarAnimator> ().UpdateDotPosition (_currentStageID);
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
