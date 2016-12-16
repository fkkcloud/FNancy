using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeTimer : GameMode {

	Material mat;
	Renderer[] renders;

	public override void Init(){
		base.Init ();
	}

	// only game mode timer overide this for now to have the timer increase from 0.0!
	public override void SetTimer(){
		float timerDisplay = Mathf.Max (0.0f, (_timer));
		if (_timer < _timeLimit + 0.1f) {
			timerDisplay = Mathf.Min (_timeLimit, (_timer));
		}
			
		currentStage.UITextMeshTimer.text = timerDisplay.ToString("0.0");
	}


	public override void Tick() {
		base.Tick ();

		if (!_timerOn)
			return;

		float a = _timeLimit - globalVariables.a;
		float b = _timeLimit - globalVariables.b;
		float c = _timeLimit + globalVariables.c + globalVariables.d;

		if (_timer >= c) {
			gameState.GameOver ();
		}
		
		if (gameCharacter._state == GameCharacter.CurrentState.Undefeatable) {
			gameCharacter.currentUndefeatCount += 1;
			gameCharacter.UpdateCharacterState ();

			gameState.PlayPerfectSound (); 
			gameState.StageClear ();
		}

		// coloring the text
		Color textClr = mat.color;
		if (_timer >= a && _timer < b) { 		// GOOD
			textClr = new Color (0.9f, 0.7f, 0.2f);
		} else if (_timer >= b && _timer < c) { // PERFECT
			textClr = new Color (0.2f, 0.9f, 0.2f);
		} else if (_timer >= c){ 				// GAME OVER
			textClr = new Color (0.9f, 0.2f, 0.2f);
		}
		mat.color = textClr;
		// color bomb
		foreach (Renderer r in renders) {
			if (r.gameObject.tag == "Indicator") {

				/*
				Color clr = r.material.GetColor ("_Color");
				r.material.SetColor ("_Color", new Color (clr.r + 0.014f, clr.g, clr.b));
				*/

				Color clr = r.material.color;

				if (_timer >= a && _timer < b) { 		// GOOD
					clr = new Color (0.1f, 0.4f, 0.1f);
				} else if (_timer >= b && _timer < c) { // PERFECT
					clr = new Color (0.1f, 0.4f, 0.1f);
				} else if (_timer >= c){ 				// GAME OVER
					clr = new Color (0.9f, 0.2f, 0.2f);
				}
				r.material.color = clr;
			}


		}
	}

	public override void ReactOnTouch() {
		base.ReactOnTouch ();

		if (!_timerOn)
			return;

		currentStage.UIElements.GameMode0_UI.SetActive (false);

		float a = _timeLimit - globalVariables.a;
		float b = _timeLimit - globalVariables.b;
		float c = _timeLimit + globalVariables.c + globalVariables.d;

		// when player does not do anything
		if (_timer >= a && _timer < c) {

			if (_timer >= b) {
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
		currentStage.cageObj.SetActive (true);

		mat = currentStage.UITextMeshTimer.GetComponent<MeshRenderer> ().material;
		renders = currentStage.cageObj.GetComponentsInChildren<Renderer> ();

		// setup UI
		currentStage.gameUIList.Add (currentStage.UITextMeshTimer.gameObject);
		currentStage.gameUIList.Add (currentStage.UITextMeshTimerIndicator.gameObject);
		currentStage.gameUIList.Add (currentStage.UIElements.GameMode0_UI);
	}
}
