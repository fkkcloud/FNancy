using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeTimer : GameMode {

	public override void Init(){
		base.Init ();
	}

	// only game mode timer overide this for now to have the timer increase from 0.0!
	public override void CalculateTimer(){
		float timerDisplay = Mathf.Max (0.0f, (_timer));
		currentStage.UITextMeshTimer.text = timerDisplay.ToString("0.0");
	}


	public override void Tick() {
		base.Tick ();

		if (!_timerOn)
			return;
		
		if (gameCharacter._state == GameCharacter.CurrentState.Undefeatable) {
			gameCharacter.currentUndefeatCount += 1;
			gameCharacter.UpdateCharacterState ();

			gameState.PlayPerfectSound (); 
			gameState.StageClear ();
		}

		// coloring the text
		if (_timer < _timeLimit - 0.25f) {
			currentStage.UITextMeshTimer.color = new Color (0.9f, 0.2f, 0.2f);
		} else if (_timer >= _timeLimit - 0.25f && _timer < _timeLimit - 0.125f) { // GOOD
			currentStage.UITextMeshTimer.color = new Color (0.9f, 0.9f, 0.2f);
		} else if (_timer > _timeLimit + 0.075f) { // GAME OVER
			currentStage.UITextMeshTimer.color = new Color (0.9f, 0.2f, 0.2f);
		} else if (_timer >= _timeLimit - 0.125f && _timer < _timeLimit + 0.075f){ // PERFECT
			currentStage.UITextMeshTimer.color = new Color (0.2f, 0.9f, 0.2f);
		}

		// color bomb
		foreach (Renderer r in currentStage.bombObj.GetComponentsInChildren<Renderer>()) {
			if (r.gameObject.tag == "PlayMat")
				continue;
			Color c = r.material.GetColor ("_Color");
			r.material.SetColor ("_Color", new Color (c.r + 0.014f, c.g, c.b));

		}
	}

	public override void ReactOnTouch() {
		base.ReactOnTouch ();

		if (!_timerOn)
			return;

		currentStage.UIElements.GameMode0_UI.SetActive (false);

		if (_timer >= _timeLimit - 0.25f && _timer < _timeLimit + 0.075f) {

			if (_timer >= _timeLimit - 0.125f && _timer < _timeLimit + 0.075f) {
				gameState.PlayTextFeedBack (GameState.FeedbackType.Perfect);
				gameState.PlayPerfectSound ();
				gameCharacter.currentPerfect += 1;
				gameCharacter.UpdateCharacterState ();
			} else {
				gameState.PlayRegularClearSound ();
				gameCharacter.StateReset ();
			}
				
			gameState.StageClear ();
		} else {
			gameState.GameOver ();
		}
	}

	public override void StartGame(){
		base.StartGame ();
	}

	public override void StopGame(){
		base.StopGame ();
	}

	public override void SetupUI(){

		// setup UI
		currentStage.gameUIList.Add (currentStage.UITextMeshTimer.gameObject);
		currentStage.gameUIList.Add (currentStage.UITextMeshTimerIndicator.gameObject);
		currentStage.gameUIList.Add (currentStage.UIElements.GameMode0_UI);
	}
}
