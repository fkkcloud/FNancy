using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSlider : GameMode {

	public GameObject slider;
	public float rightEndX;
	private LTDescr _sliderAnimation;
	private int _stageHP;
	public int currentDamage;

	public override void Init(){
		base.Init ();
		currentDamage = 0;
		_stageHP = currentStage.stageData.hp;
	}

	public override void Tick() {
		base.Tick ();

		if (!_timerOn)
			return;

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
	}

	public override void ReactOnTouch() {
		base.ReactOnTouch ();

		if (!_timerOn)
			return;

		if (_stageHP > currentDamage) {

			gameState.PlayHitSound ();

			for (int i = 0; i < currentStage.minigamePlayUI.Count; i++) {
				float x_diff = Mathf.Abs(slider.transform.position.x - currentStage.minigamePlayUI [i].transform.position.x);
				if (x_diff < 0.08666f) {
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
		}
	}

	public override void StartGame(){
		base.StartGame ();
		float mostRightX = 0.39f; // TEMP : TODO - have to get value procedurally
		_sliderAnimation = LeanTween.moveLocalX (slider, mostRightX, 0.5f).setLoopPingPong(0);
	}

	public override void StopGame(){
		base.StopGame ();
		_sliderAnimation.pause (); // maybe stop for forever..?
		slider.SetActive (false);
	}

	public override void SetupUI(){

		// setup UI
		currentStage.gameUIList.Add (currentStage.UITextMeshTimer.gameObject);
		//_gameUI.Add (UITextMeshTimerIndicator.gameObject);
		currentStage.gameUIList.Add (currentStage.UIElements.GameMode2_UI);

		float playMatLength = currentStage.UIElements.PlayMat.transform.lossyScale.x;
		Vector3 playMatPos = currentStage.UIElements.PlayMat.transform.position;

		float playMatMostLeftX = playMatPos.x - playMatLength * 0.5f;
		float playMatMostRightX = playMatPos.x + playMatLength * 0.5f;

		float div = playMatLength / (currentStage.stageData.hp + 1);
		float scale = div * 0.666f;

		for (int i = 0; i < currentStage.stageData.hp; i++) {

			float val = Random.Range (playMatMostLeftX + playMatLength * 0.1f, playMatMostRightX - playMatLength * 0.1f);
			Vector3 pos = new Vector3 (val, playMatPos.y, playMatPos.z);
			GameObject dot = Instantiate (currentStage.UIElements.UI_DOT);

			dot.transform.position = pos;
			dot.transform.localScale = new Vector3 (scale * 0.4f, scale * 0.8f, scale);
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
