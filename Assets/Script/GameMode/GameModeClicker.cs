using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeClicker : GameMode {

	public int CurrentHP;

	public override void Init(){
		base.Init ();

		CurrentHP = 0;

		Debug.Log ("HP:" + CurrentHP);
	}

	public override void Tick() {
		base.Tick ();

		// color bomb
		foreach (Renderer r in currentStage.bombObj.GetComponentsInChildren<Renderer>()) {
			Color c = r.material.GetColor ("_Color");
			r.material.SetColor ("_Color", new Color (c.r, c.g, c.b + 0.02f));
		}
	}

	public override void Act() {
		base.Act ();

		if (currentStage.stageData.hp > CurrentHP) {
			CurrentHP++;
			gameState.PlayHitSound ();
			gameState.PlayHitFX ();
		} else {
			gameState.PlayPerfectSound ();
			gameState.StageClear ();
		}  
	}
}