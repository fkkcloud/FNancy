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

	public override void Act() {
		base.Act ();

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

}