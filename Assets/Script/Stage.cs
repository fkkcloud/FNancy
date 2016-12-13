using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviourHelper
{
	public enum AnimType {BombDown, Activate, BombShake, GameOver};

	public StageData stageData;

	public GameMode gameMode;
	public List<GameObject> gameUIList = new List<GameObject> ();

	private GameObject _stageObj;
	public GameObject stageObj
	{
		get
		{
			return _stageObj;
		}
		set 
		{ 
			_stageObj = value;
		}
	}

	private GameObject _bombObj;
	public GameObject bombObj
	{
		get
		{
			return _bombObj;
		}
		set
		{
			_bombObj = value;
		}
	}

	private GameObject _perfectText;
	public GameObject UIPerfect
	{
		get
		{
			return _perfectText;
		}
		set
		{
			_perfectText = value;
		}
	}

	public TextMesh UITextMeshTimerIndicator;
	public TextMesh UITextMeshTimer;

	private Animator _anim;
	public StageElements UIElements;

	// UI
	public List<GameObject> minigamePlayUI = new List<GameObject>(); // used to eval any minigame UIs

	public void Awake(){
		
	}

	public void Animate(AnimType type){
		if (type == AnimType.BombDown) {
			_anim.SetTrigger ("BombDownTrigger");
		} else if (type == AnimType.Activate) {
			_anim.SetTrigger ("ActivateTrigger");
		} else if (type == AnimType.BombShake) {
			_anim.SetTrigger ("BombShakeTrigger");
		}
	}

	public void Init(StageData data, ref GameObject StageObject, Vector3 pos){

		stageData = data;

		stageObj = Instantiate (StageObject);
		stageObj.transform.position = pos;
		stageObj.SetActive (false);

		_anim = stageObj.GetComponent<Animator> ();
		UIElements = stageObj.GetComponent<StageElements> ();

		// SET UI stuff for stage
		UITextMeshTimer = UIElements.TextBombTimer.GetComponent<TextMesh>();
		UITextMeshTimer.text = "";
		UITextMeshTimer.gameObject.SetActive (false);

		UITextMeshTimerIndicator = UIElements.TextTimerIndicator.GetComponent<TextMesh>();
		UITextMeshTimerIndicator.text = stageData.timeLimit.ToString("0.0");

		UITextMeshTimerIndicator.gameObject.SetActive (false);

		UIPerfect = UIElements.PerfectText;
		UIPerfect.SetActive (false);

		bombObj = UIElements.BombObj;

		InitGameMode ();
	}

	public void DestroyStageObject(){
		DestroyImmediate (stageObj);
	}

	public void Activate(){
		stageObj.SetActive (true);
		Animate(Stage.AnimType.Activate);
		gameMode.Init ();
	}

	public void DeActivate(){
		Animate(Stage.AnimType.BombDown);
		gameMode.StopGame ();
	}

	public void HideBomb(){
		bombObj.SetActive (false);
	}

	public void AnimatePerfectFeedBack(){
		UIPerfect.transform.position = new Vector3 (4f, UIPerfect.transform.position.y, UIPerfect.transform.position.z);
		LeanTween.moveLocalX (UIPerfect, -5.5f, 0.38f);
	}

	public float GetCurrentLimitTime()
	{
		return stageData.timeLimit;
	}

	public void UpdateIndicator()
	{
		UITextMeshTimerIndicator.text = stageData.timeLimit.ToString();
	}

	void InitGameMode(){
		if (stageData.gamemode == 0) {
			gameMode = gameModeTimer;
		}
		else if (stageData.gamemode == 1){
			gameMode = gameModeClicker;
		} 
		else if (stageData.gamemode == 2){
			gameMode = gameModeSlider;
		}
		else if (stageData.gamemode == 3){
			gameMode = gameModeColor;
		}
	}

	public void ToggleUIVisibility(bool val){
		for (int i = 0; i < gameUIList.Count; i++) {
			gameUIList [i].SetActive (val);
		}
	}

	public void AnimatePerfectUI()
	{
		_perfectText.transform.position = new Vector3 (4f, _perfectText.transform.position.y, _perfectText.transform.position.z);
		LeanTween.moveLocalX (_perfectText, -5.5f, 0.38f);
	}
}
