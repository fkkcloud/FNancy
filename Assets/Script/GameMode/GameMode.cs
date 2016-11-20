using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameMode : MonoBehaviourHelper {

	protected float _timer;
	protected bool _timerOn;
	protected float _timeLimit;

	public virtual void Init(){
		_timerOn = false;
		_timer = 0f;
		_timeLimit = currentStage.GetCurrentLimitTime ();

		// turn on corresponding UI elems
		currentStage.ToggleUIVisibility (true);
	}

	public virtual void Tick (){
		if (_timerOn) {
			
			// when player does not do anything
			if (_timer > _timeLimit + 0.05f) {
				gameState.GameOver ();
			}

			// update timer
			_timer += (gameDesignVariables.TimerSpeed * Time.deltaTime);
			currentStage.UITextMeshTimer.text = _timer.ToString("0.0"); // TODO : make it as function for polyomrphism

			if (_timer > _timeLimit - 0.35f) {
				currentStage.Animate (Stage.AnimType.BombShake);
			}

			if (gameCharacter._state == GameCharacter.CurrentState.Undefeatable) {
				gameCharacter.currentUndefeatCount += 1;
				gameCharacter.UpdateCharacterState ();

				gameState.PlayPerfectSound (); 
				gameState.StageClear ();
			}
		}
	}

	public virtual void Act (){
		currentStage.ActUpdateUI ();
	}

	public virtual void StartGame(){ 
		_timerOn = true; 
	}
	public virtual void StopGame(){
		_timerOn = false;
		currentStage.Animate (Stage.AnimType.BombShake); // trigger it to stop shaking
	}


}
