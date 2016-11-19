using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameMode : MonoBehaviourHelper {

	protected float _timer;
	public bool timerOn;
	protected float _timeLimit;

	public virtual void Init(){
		_timer = 0f;
		timerOn = false;
		_timeLimit = currentStage.GetCurrentLimitTime ();
	}

	public virtual void Tick (){

		//Debug.Log ("base tick");

		if (timerOn) {

			//Debug.Log ("base tick timerOn");

			// when player does not do anything
			if (_timer > _timeLimit + 0.05f) {
				gameState.GameOver ();
			}

			// update timer
			_timer += (gameDesignVariables.TimerSpeed * Time.deltaTime);
			currentStage.TextMeshTimer.text = _timer.ToString("0.0"); // TODO : make it as function for polyomrphism

			if (_timer > _timeLimit - 0.28f) {
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
		
	}

	public void StopGame(){
		timerOn = false;
		currentStage.Animate (Stage.AnimType.BombShake); // trigger it to stop shaking
	}

}
