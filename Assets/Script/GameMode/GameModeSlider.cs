using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSlider : GameMode {

	public GameObject slider;
	public float rightEndX;
	private LTDescr _sliderAnimation;
	private int _stageHP;
	public int currentDamage;

	private float x_size_half;
	private float x_size_half_factor = 1.86f;

	public override void Init(){
		base.Init ();
		currentDamage = 0;
		_stageHP = currentStage.stageData.hp;
	}

	public override void Tick() {
		base.Tick ();

		if (!_timerOn)
			return;

		// when player does not do anything
		if (_timer > _timeLimit) {
			gameState.GameOver ();
		}

		if (gameCharacter._state == GameCharacter.CurrentState.Undefeatable) {
			gameCharacter.StateReset ();
		}

		// color bomb
		foreach (Renderer r in currentStage.bombObj.GetComponentsInChildren<Renderer>()) {
			if (r.gameObject.tag == "PlayMat")
				continue;
			Color c = r.material.GetColor ("_Color");
			r.material.SetColor ("_Color", new Color (c.r, c.g + 0.014f, c.b));
		}

		Color clr = new Color (0.9f, 0.9f, 0.9f, 1f);
		for (int i = 0; i < currentStage.minigamePlayUI.Count; i++) {
			float x_diff = Mathf.Abs(slider.transform.position.x - currentStage.minigamePlayUI [i].transform.position.x);
			if (x_diff < x_size_half * x_size_half_factor) {
				clr = new Color (0.2f, 0.9f, 0.2f, 1f);
			}
		}
		slider.GetComponent<MeshRenderer> ().material.SetColor ("_Color", clr);
	}

	public override void ReactOnTouch() {
		base.ReactOnTouch ();

		if (!_timerOn)
			return;

		if (_stageHP > currentDamage) {

			gameState.PlayHitSound ();

			bool hitGood = false;
			for (int i = 0; i < currentStage.minigamePlayUI.Count; i++) {
				float x_diff = Mathf.Abs(slider.transform.position.x - currentStage.minigamePlayUI [i].transform.position.x);
				if (x_diff < x_size_half * x_size_half_factor) {
					hitGood = true;
					currentDamage++;
					currentStage.minigamePlayUI [i].SetActive (false);
					currentStage.minigamePlayUI.Remove (currentStage.minigamePlayUI [i]);
					//gameState.PlayHitFX (currentStage.gamePlayUI [i].transform.position);
					if (_stageHP <= currentDamage) {
						gameState.PlayPerfectSound ();
						gameState.StageClear ();
						break;
					}
				}
			}
			if (!hitGood) {
				gameState.GameOver ();
			}
		}
	}

	public override void StartGame(){
		base.StartGame ();
		float mostRightX = 0.39f; // TEMP : TODO - have to get value procedurally
		_sliderAnimation = LeanTween.moveLocalX (slider, mostRightX, 1f).setLoopPingPong(0);
	}

	public override void StopGame(){
		base.StopGame ();
		_sliderAnimation.pause (); // maybe stop for forever..?
		slider.SetActive (false);
	}

	public override void SetupUI(){

		currentStage.bombObj.SetActive (true);

		// setup UI
		//currentStage.gameUIList.Add (currentStage.UITextMeshTimer.gameObject);
		currentStage.gameUIList.Add (currentStage.UITextMeshTimerIndicator.gameObject);
		currentStage.gameUIList.Add (currentStage.UIElements.GameMode2_UI);

		float playMatLength = currentStage.UIElements.PlayMat.transform.lossyScale.x;
		Vector3 playMatPos = currentStage.UIElements.PlayMat.transform.position;

		float playMatMostLeftX = playMatPos.x - playMatLength * 0.5f;
		float playMatMostRightX = playMatPos.x + playMatLength * 0.5f;

		float div = playMatLength / (currentStage.stageData.hp + 1);
		float scale = div * 0.666f;
		x_size_half = scale * 0.4f;
		for (int i = 0; i < currentStage.stageData.hp; i++) {

			float val = Random.Range (playMatMostLeftX + playMatLength * 0.1f, playMatMostRightX - playMatLength * 0.1f);
			Vector3 pos = new Vector3 (val, playMatPos.y, playMatPos.z);
			GameObject dot = Instantiate (currentStage.UIElements.UI_DOT);

			dot.transform.position = pos;
			dot.transform.localScale = new Vector3 (x_size_half, scale * 0.8f, scale);
			dot.transform.parent = currentStage.UIElements.GameMode2_UI.transform;
			currentStage.minigamePlayUI.Add (dot);
		}

		slider = Instantiate (currentStage.UIElements.UI_CYLINDER);
		Vector3 sliderPosition = new Vector3 (playMatMostLeftX + playMatLength * 0.1f, playMatPos.y, playMatPos.z);

		slider.transform.position = sliderPosition;

		rightEndX = playMatMostRightX - playMatLength * 0.1f;
		slider.transform.parent = currentStage.UIElements.GameMode2_UI.transform;
	}
}
