using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeTimer : GameMode {

	public override void Init(){
		base.Init ();
	}

	public override void Tick() {

		base.Tick ();

		//Debug.Log ("game timer tick");

		if (timerOn) {

			//Debug.Log ("game timer tick timerOn");

			// coloring the text
			if (_timer < _timeLimit - 0.25f) {
				currentStage.TextMeshTimer.color = new Color (0.9f, 0.2f, 0.2f);
			} else if (_timer >= _timeLimit - 0.25f && _timer <= _timeLimit - 0.1f) {
				currentStage.TextMeshTimer.color = new Color (0.9f, 0.9f, 0.2f);
			} else if (_timer > _timeLimit + 0.05f) {
				currentStage.TextMeshTimer.color = new Color (0.9f, 0.2f, 0.2f);
			} else {
				currentStage.TextMeshTimer.color = new Color (0.2f, 0.9f, 0.2f);
			}

			// color bomb
			foreach (Renderer r in currentStage.bombObj.GetComponentsInChildren<Renderer>()) {
				Color c = r.material.GetColor ("_Color");
				r.material.SetColor ("_Color", new Color (c.r + 0.014f, c.g, c.b));

			}

		}
	}

	public override void Act() {
		base.Act ();
		if (_timer >= _timeLimit - 0.25f && _timer < _timeLimit + 0.05f) {

			if (_timer >= _timeLimit - 0.1f && _timer < _timeLimit + 0.05f) {
				currentStage.ShowPerfectUI ();
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


}
