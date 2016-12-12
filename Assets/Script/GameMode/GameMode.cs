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

			// update timer
			float timeMult = stageManager.GetCurrentStageTimeMult();
			_timer += (gameDesignVariables.TimerSpeed * timeMult * Time.deltaTime);
			
			// when player does not do anything
			if (_timer > _timeLimit + 0.15f) {
				gameState.GameOver ();
			}

			if (_timer > _timeLimit - 0.35f) {
				currentStage.Animate (Stage.AnimType.BombShake);
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
