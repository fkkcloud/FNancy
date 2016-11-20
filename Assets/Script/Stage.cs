using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviourHelper
{
	public enum AnimType {BombDown, Activate, BombShake, GameOver};

	public StageData stageData;

	public GameMode gameMode;

	private List<GameObject> _gameUI = new List<GameObject> ();

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
	private StageElements _elems;

	// UI
	public List<GameObject> gamePlayUI = new List<GameObject>();

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
		_elems = stageObj.GetComponent<StageElements> ();

		// SET UI stuff for stage
		UITextMeshTimer = _elems.TextBombTimer.GetComponent<TextMesh>();
		UITextMeshTimer.text = "";
		UITextMeshTimer.gameObject.SetActive (false);

		UITextMeshTimerIndicator = _elems.TextTimerIndicator.GetComponent<TextMesh>();
		UITextMeshTimerIndicator.text = stageData.timeLimit.ToString();
		UITextMeshTimerIndicator.gameObject.SetActive (false);

		UIPerfect = _elems.PerfectText;
		UIPerfect.SetActive (false);

		bombObj = _elems.BombObj;

		InitGameMode ();
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
		if (stageData.gamemode == 0) 
		{
			gameMode = gameModeTimer;

			// setup UI
			_gameUI.Add (UIPerfect);
			_gameUI.Add (UITextMeshTimer.gameObject);
			_gameUI.Add (UITextMeshTimerIndicator.gameObject);
			_gameUI.Add (_elems.GameMode0_UI);
		} 
		else if (stageData.gamemode == 1) {
			gameMode = gameModeClicker;

			// setup UI
			_gameUI.Add (UITextMeshTimer.gameObject);
			//_gameUI.Add (UITextMeshTimerIndicator.gameObject);
			_gameUI.Add (_elems.GameMode1_UI);

			float playMatLength = _elems.PlayMat.transform.lossyScale.x;
			Vector3 playMatPos = _elems.PlayMat.transform.position;
			float playMatMostLeftX = playMatPos.x - playMatLength * 0.5f;
			for (int i = 0; i < stageData.hp; i++) {
				float div = playMatLength / (stageData.hp + 1);
				float scale = div * 0.666f;

				Vector3 pos = new Vector3 (playMatMostLeftX + div * (i+1), playMatPos.y, playMatPos.z);
				GameObject dot = Instantiate (_elems.UI_DOT);

				dot.transform.position = pos;
				dot.transform.localScale = new Vector3 (scale, scale, scale);
				dot.transform.parent = _elems.GameMode1_UI.transform;
				gamePlayUI.Add (dot);
			}
		} 
		else if (stageData.gamemode == 2)
		{
			gameMode = gameModeSlider;

			// setup UI
			_gameUI.Add (UITextMeshTimer.gameObject);
			//_gameUI.Add (UITextMeshTimerIndicator.gameObject);
			_gameUI.Add (_elems.GameMode2_UI);

			float playMatLength = _elems.PlayMat.transform.lossyScale.x;
			Vector3 playMatPos = _elems.PlayMat.transform.position;

			float playMatMostLeftX = playMatPos.x - playMatLength * 0.5f;
			float playMatMostRightX = playMatPos.x + playMatLength * 0.5f;

			float div = playMatLength / (stageData.hp + 1);
			float scale = div * 0.666f;

			for (int i = 0; i < stageData.hp; i++) {

				float val = Random.Range (playMatMostLeftX + playMatLength * 0.1f, playMatMostRightX - playMatLength * 0.1f);
				Vector3 pos = new Vector3 (val, playMatPos.y, playMatPos.z);
				GameObject dot = Instantiate (_elems.UI_DOT);

				dot.transform.position = pos;
				dot.transform.localScale = new Vector3 (scale * 0.4f, scale * 0.8f, scale);
				dot.transform.parent = _elems.GameMode2_UI.transform;
				gamePlayUI.Add (dot);
			}

			GameObject slider = Instantiate (_elems.UI_CYLINDER);
			Vector3 sliderPosition = new Vector3 (playMatMostLeftX + playMatLength * 0.1f, playMatPos.y, playMatPos.z);

			slider.transform.position = sliderPosition;

			GameModeSlider gml = gameMode as GameModeSlider;
			gml.slider = slider;
			gml.rightEndX = playMatMostRightX - playMatLength * 0.1f;
			slider.transform.parent = _elems.GameMode2_UI.transform;
		}
	}

	public void ActUpdateUI(){
		if (stageData.gamemode == 0) {
			_elems.GameMode0_UI.SetActive (false);
		} else if (stageData.gamemode == 1) {
			GameModeClicker gmc = gameMode as GameModeClicker;
			if (gmc.currentDamage < stageData.hp)
				gamePlayUI [gmc.currentDamage].SetActive (false);
		} else if (stageData.gamemode == 2){
			
		}
	}

	public void ToggleUIVisibility(bool val){
		for (int i = 0; i < _gameUI.Count; i++) {
			_gameUI [i].SetActive (val);
		}
	}

	public void AnimatePerfectUI()
	{
		_perfectText.transform.position = new Vector3 (4f, _perfectText.transform.position.y, _perfectText.transform.position.z);
		LeanTween.moveLocalX (_perfectText, -5.5f, 0.38f);
	}
}
