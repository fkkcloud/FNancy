using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviourHelper
{
	public enum AnimType {BombDown, Activate, BombShake, GameOver};

	public StageData stageData;

	public GameMode gameMode;

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
	public GameObject perfectText
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

	public TextMesh TextMeshTimerIndicator;
	public TextMesh TextMeshTimer;


	private Animator _anim;

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
		_anim = stageObj.GetComponent<Animator> ();

		// reset 3d text meshes to none
		TextMesh[] childrens = stageObj.GetComponentsInChildren<TextMesh>();
		foreach (TextMesh child in childrens) {
			// do what you want with the transform
			if (child.gameObject.tag == "TimerBomb") {
				child.text = "";
			} else if (child.gameObject.tag == "TimerStage") {
				child.text = "";
			}
		}

		foreach(Transform child in stageObj.transform){
			if(child.gameObject.tag == "BombObject"){
				bombObj = child.gameObject;
				break;
			}
		}

		foreach(Transform child in stageObj.transform){
			if(child.gameObject.tag == "PerfectText"){
				perfectText = child.gameObject;
				break;
			}
		}

		InitText ();
		InitGameMode ();
	}

	public void Activate(){
		stageObj.SetActive (true);
		Animate(Stage.AnimType.Activate);
		gameMode.Init ();

	}

	public void DeActivate(){
		Animate(Stage.AnimType.BombDown);
		HideTexts ();
		gameMode.StopGame ();
	}

	public void HideBomb(){
		bombObj.SetActive (false);
	}

	public void AnimatePerfectFeedBack(){
		perfectText.transform.position = new Vector3 (4f, perfectText.transform.position.y, perfectText.transform.position.z);
		LeanTween.moveLocalX (perfectText, -5.5f, 0.38f);
	}

	public float GetCurrentLimitTime()
	{
		return stageData.timeLimit;
	}

	public void UpdateIndicator()
	{
		TextMeshTimerIndicator.text = stageData.timeLimit.ToString();
	}
		
	private void InitText()
	{
		TextMesh[] _textMeshes = stageObj.GetComponentsInChildren<TextMesh>();;

		foreach (TextMesh child in _textMeshes) {
			// do what you want with the transform
			if (child.gameObject.tag == "TimerBomb") {
				child.text = 0.0f.ToString("#.#");
				child.gameObject.SetActive (true);
				TextMeshTimer = child;
			} else if (child.gameObject.tag == "TimerStage") {
				child.text = stageData.timeLimit.ToString();
				child.gameObject.SetActive (true);
				TextMeshTimerIndicator = child;
			}
		}

		_perfectText = stageObj.transform.FindChild ("PerfectText").gameObject;
	}

	public void InitGameMode(){
		if (stageData.gamemode == 0)
			gameMode = gameModeTimer;
		else if (stageData.gamemode == 1)
			gameMode = gameModeClicker;
	}

	public void HideTexts(){
		TextMeshTimer.gameObject.SetActive (false);
		TextMeshTimerIndicator.gameObject.SetActive (false);
	}

	public void ShowPerfectUI()
	{
		_perfectText.transform.position = new Vector3 (4f, _perfectText.transform.position.y, _perfectText.transform.position.z);
		LeanTween.moveLocalX (_perfectText, -5.5f, 0.38f);
	}
}
