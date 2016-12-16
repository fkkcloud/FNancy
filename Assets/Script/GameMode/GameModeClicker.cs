using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeClicker : GameMode {

	public int currentDamage;
	private int _stageHP;

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
			r.material.SetColor ("_Color", new Color (c.r, c.g, c.b + 0.02f));
		}
	}

	public override void ReactOnTouch() {
		base.ReactOnTouch ();

		if (!_timerOn)
			return;

		if (currentDamage < currentStage.stageData.hp)
			currentStage.minigamePlayUI [currentDamage].SetActive (false);

		if (_stageHP > currentDamage) {
			currentDamage++;
			gameState.PlayHitSound ();
			gameState.PlayHitFX (new Vector3(1.1f, 0.66f, -2.86f));
			if (_stageHP <= currentDamage) {
				gameState.PlayPerfectSound ();
				gameState.StageClear ();
			}
		}
	}

	public override void StartGame(){
		base.StartGame ();
	}

	public override void StopGame(){
		base.StopGame ();
	}

	public override void SetupUI(){

		currentStage.bombObj.SetActive (true);

		// setup UI
		currentStage.gameUIList.Add (currentStage.UITextMeshTimerIndicator.gameObject);
		currentStage.gameUIList.Add (currentStage.UIElements.GameMode1_UI);

		float playMatLength = currentStage.UIElements.PlayMat.transform.lossyScale.x;
		Vector3 playMatPos = currentStage.UIElements.PlayMat.transform.position;
		float playMatMostLeftX = playMatPos.x - playMatLength * 0.5f;
		for (int i = 0; i < currentStage.stageData.hp; i++) {
			float div = playMatLength / (currentStage.stageData.hp + 1);
			float scale = div * 0.666f;

			Vector3 pos = new Vector3 (playMatMostLeftX + div * (i+1), playMatPos.y, playMatPos.z);
			GameObject dot = Instantiate (currentStage.UIElements.UI_DOT);

			dot.transform.position = pos;
			dot.transform.localScale = new Vector3 (scale, scale, scale);
			dot.transform.parent = currentStage.UIElements.GameMode1_UI.transform;
			currentStage.minigamePlayUI.Add (dot);
		}
	}
}