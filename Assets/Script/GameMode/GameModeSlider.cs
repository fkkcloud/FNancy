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

	public override void Act() {
		base.Act ();

		if (_stageHP > currentDamage) {

			gameState.PlayHitSound ();

			for (int i = 0; i < currentStage.gamePlayUI.Count; i++) {
				float x_diff = Mathf.Abs(slider.transform.position.x - currentStage.gamePlayUI [i].transform.position.x);
				if (x_diff < 0.08666f) {
					currentDamage++;
					currentStage.gamePlayUI [i].SetActive (false);
					currentStage.gamePlayUI.Remove (currentStage.gamePlayUI [i]);
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
}
