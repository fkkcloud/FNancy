﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameState : MonoBehaviour {

	public TextAsset StageJson;
	public GameObject StageObject;
	public LevelManager LevelManager;
	public GameObject ClearText;
	public float StageLength = 4.5f;
	public GameObject Audio;

	public float TimerSpeed = 0.8f;

	private StageData[] _stageData;
	private float _currentTime;
	private float _currentStageTime;
	private int _currentStageID;
	private GameObject[] _stageInstances;
	private Animator _anim;
	private TextMesh _currentTextMeshBombTimer;
	private TextMesh _currentTextMeshStageTimer;

	private float timer = 0f;
	private bool timerOn = true;

	private GameObject _prevStage;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (timerOn) {
			timer += (TimerSpeed * Time.deltaTime);

			// when player does not do anything
			if (timer > _currentStageTime + 0.5) {
				// go to main menu
				LevelManager.LoadMainMenu();
			}

			// update timer
			if (_currentTextMeshBombTimer != null)
				_currentTextMeshBombTimer.text = timer.ToString("0.0");


			// touch on screen
			if (Input.GetMouseButtonDown (0))
			{
				//Debug.Log("Touched");
				if (timer >= _currentStageTime - 0.5f && timer <= _currentStageTime + 0.5f)
					GoToNextStage ();
				else
					LevelManager.LoadMainMenu();
			}
		}
		   
	}

	void OnLevelWasLoaded(int level){
		InitStages ();
	}

	void InitStages(){
		_stageData = StaticUtils.CreateStages (StageJson);
		_stageInstances = new GameObject[_stageData.Length];

		float ZPos = 0.0f;
		for (int i = 0; i < _stageData.Length; i++) 
		{
			GameObject StageInstance = Instantiate (StageObject);
			StageInstance.transform.position = new Vector3(0, 0, ZPos);
			ZPos += StageLength;

			// reset text to
			TextMesh[] childrens = StageInstance.GetComponentsInChildren<TextMesh>();
			foreach (TextMesh child in childrens) {
				// do what you want with the transform
				if (child.gameObject.tag == "TimerBomb") {
					child.text = "";
				} else if (child.gameObject.tag == "TimerStage") {
					child.text = "";
				}
			}

			_stageInstances [i] = StageInstance;
		}

		_currentStageID = 0;
		_currentStageTime = _stageData [_currentStageID].time;


		TextMesh[] children = _stageInstances[_currentStageID].GetComponentsInChildren<TextMesh>();
		foreach (TextMesh child in children) {
			// do what you want with the transform
			if (child.gameObject.tag == "TimerBomb") {
				_currentTextMeshBombTimer = child;
				_currentTextMeshBombTimer.text = 0.0f.ToString ("#.#");
				_currentTextMeshBombTimer.gameObject.SetActive (true);
			} else if (child.gameObject.tag == "TimerStage") {
				_currentTextMeshStageTimer = child;
				_currentTextMeshStageTimer.text = _stageData [_currentStageID].time.ToString();
				_currentTextMeshStageTimer.gameObject.SetActive (true);
			}
		}
	}

	void GoToNextStage(){
		//if (_stageInstances.Length < _currentStageID + 2)
		//	return;
		if (Audio)
			Audio.GetComponent<AudioSource> ().Play ();

		if (_stageInstances.Length - 1 == _currentStageID) {
			HandleClear ();
			return;
		}

		// disable prev stage inactive
		if (_currentStageID - 1 > -1) {
			_prevStage = _stageInstances [_currentStageID - 1];
		}

		// animate current stage 
		Animator Anim = _stageInstances[_currentStageID].GetComponent<Animator> ();
		Anim.SetTrigger ("BombDownTrigger");
		_currentTextMeshBombTimer.gameObject.SetActive (false);
		_currentTextMeshStageTimer.gameObject.SetActive (false);

		// update current stage to next
		_currentStageID += 1;
		_currentStageTime = _stageData [_currentStageID].time;

		// activate next stage
		_stageInstances [_currentStageID].SetActive (true);

		// animate next stage
		Anim = _stageInstances[_currentStageID].GetComponent<Animator> ();
		Anim.SetTrigger ("ActivateTrigger");

		// move all the stages
		float ZPos = StageLength;
		float MoveSpeed = 1.0f;
		for (int i = 0; i < _stageData.Length; i++) {
			GameObject CurrStage = _stageInstances [i];
			float newZPos = CurrStage.transform.position.z - ZPos;
			LeanTween.moveZ (CurrStage, newZPos, MoveSpeed).setEase (LeanTweenType.easeInQuad);
		}
		Invoke ("DisablePrevStage", MoveSpeed * 2f);
		Invoke ("EnableTimer", MoveSpeed * 1f);

		timer = 0f;
		timerOn = false;

		TextMesh[] childrens = _stageInstances[_currentStageID].GetComponentsInChildren<TextMesh>();
		foreach (TextMesh child in childrens) {
			// do what you want with the transform
			if (child.gameObject.tag == "TimerBomb") {
				_currentTextMeshBombTimer = child;
				_currentTextMeshBombTimer.text = 0.0f.ToString("#.#");
				_currentTextMeshBombTimer.gameObject.SetActive (true);
			} else if (child.gameObject.tag == "TimerStage") {
				_currentTextMeshStageTimer = child;
				_currentTextMeshStageTimer.text = _stageData [_currentStageID].time.ToString();
				_currentTextMeshStageTimer.gameObject.SetActive (true);
			}
		}
	}

	void DisablePrevStage()
	{
		if (_prevStage)
			_prevStage.SetActive (false);
	}

	void EnableTimer()
	{
		timerOn = true;
	}

	void HandleClear()
	{
		_stageInstances [_currentStageID].SetActive (false);
		ClearText.SetActive (true);
	}
}