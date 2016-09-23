using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameState : MonoBehaviour {

	public TextAsset StageJson;
	public GameObject StageObject;
	public LevelManager LevelManager;

	private StageData[] _stageData;
	private float _currentTime;
	private float _currentStageTime;
	private int _currentStageID;
	private GameObject[] _stageInstances;
	private Animator _anim;
	private float _levelStartTime;
	private TextMesh _currentTextMeshBombTimer;
	private TextMesh _currentTextMeshStageTimer;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		_currentTime = Time.timeSinceLevelLoad;

		float CurrentStageElapsedTime = _currentTime - _levelStartTime;
		if (CurrentStageElapsedTime > _currentStageTime + 0.5) {
			// go to main menu
			LevelManager.LoadMainMenu();
		}

		// update timer
		if (_currentTextMeshBombTimer != null)
			_currentTextMeshBombTimer.text = CurrentStageElapsedTime.ToString("0.0");
	
			
		// touch on screen
		if (Input.GetMouseButtonDown (0))
		{
			Debug.Log("Touched");
			if (CurrentStageElapsedTime >= _currentStageTime - 0.5f && CurrentStageElapsedTime <= _currentStageTime + 0.5f)
				GoToNextStage ();
			else
				LevelManager.LoadMainMenu();
		}   
	}

	void OnLevelWasLoaded(int level){
		InitStages ();
	}

	void InitStages(){
		_stageData = StaticUtils.CreateStages (StageJson);
		_stageInstances = new GameObject[_stageData.Length];

		float ZPos = 0.0f;
		for (int i = 0; i < _stageData.Length; i++) {
			GameObject StageInstance = Instantiate (StageObject);
			StageInstance.transform.position = new Vector3(0, 0, ZPos);
			ZPos += 4.5f;
			_stageInstances [i] = StageInstance;
		}

		_currentStageID = 0;
		_currentStageTime = _stageData [_currentStageID].time;


		TextMesh[] childrens = _stageInstances[_currentStageID].GetComponentsInChildren<TextMesh>();
		foreach (TextMesh child in childrens) {

			Debug.Log (child.gameObject.tag);

			// do what you want with the transform
			if (child.gameObject.tag == "TimerBomb") {
				_currentTextMeshBombTimer = child;
				_currentTextMeshBombTimer.text = 0.0f.ToString ("#.#");;
				_currentTextMeshBombTimer.gameObject.SetActive (true);
			} else if (child.gameObject.tag == "TimerStage") {
				_currentTextMeshStageTimer = child;
				_currentTextMeshStageTimer.text = _stageData [_currentStageID].time.ToString();
				_currentTextMeshStageTimer.gameObject.SetActive (true);
			}
		}
	}

	void GoToNextStage(){
		if (_stageInstances.Length < _currentStageID + 2)
			return;

		// animate current stage 
		Animator Anim = _stageInstances[_currentStageID].GetComponent<Animator> ();
		Anim.SetTrigger ("BombDownTrigger");
		_currentTextMeshBombTimer.gameObject.SetActive (false);
		_currentTextMeshStageTimer.gameObject.SetActive (false);


		// update current stage to next
		_currentStageID += 1;
		_currentStageTime = _stageData [_currentStageID].time;

		// animate next stage
		Anim = _stageInstances[_currentStageID].GetComponent<Animator> ();
		Anim.SetTrigger ("ActivateTrigger");

		// move all the stages
		float ZPos = 4.5f;
		for (int i = 0; i < _stageData.Length; i++) {
			GameObject CurrStage = _stageInstances [i];
			float newZPos = CurrStage.transform.position.z - ZPos;
			LeanTween.moveZ (CurrStage, newZPos, 1.0f).setEase (LeanTweenType.easeInCubic);
		}

		_levelStartTime = Time.timeSinceLevelLoad;

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
}