using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeColor : GameMode {

	private string _miniGameBombTag;

	private static string[] _tags = new string[3];

	public override void Init(){
		base.Init ();

		_tags[0] = "minigame_bomb_r";
		_tags[1] = "minigame_bomb_g";
		_tags[2] = "minigame_bomb_b";

		int rand = Random.Range (0, 3);
		_miniGameBombTag = _tags [rand];


		int c_rand = Random.Range (0, 2);
		if (rand == 0) {
			currentStage.UIElements.MiniGameText.GetComponent<TextMesh> ().text = "red";
			Color[] cs = new Color[2];
			cs[0] = Color.green;
			cs[1] = Color.blue;
			currentStage.UIElements.MiniGameText.GetComponent<TextMesh> ().color = cs [c_rand];
		} else if (rand == 1) {
			currentStage.UIElements.MiniGameText.GetComponent<TextMesh> ().text = "green";
			Color[] cs = new Color[2];
			cs[0] = Color.red;
			cs[1] = Color.blue;
			currentStage.UIElements.MiniGameText.GetComponent<TextMesh> ().color = cs [c_rand];
		} else {
			currentStage.UIElements.MiniGameText.GetComponent<TextMesh> ().text = "blue";
			Color[] cs = new Color[2];
			cs[0] = Color.green;
			cs[1] = Color.red;
			currentStage.UIElements.MiniGameText.GetComponent<TextMesh> ().color = cs [c_rand];
		}
		//currentStage.UIElements.Set
	}

	public override void Tick() {
		base.Tick ();

		if (!_timerOn)
			return;

		float timerDisplay = Mathf.Max (0.0f, (currentStage.stageData.timeLimit - _timer));
		currentStage.UITextMeshTimer.text = timerDisplay.ToString("0.0"); // TODO : make it as function for polyomrphism

		// cancel highlight combo!
		if (gameCharacter._state == GameCharacter.CurrentState.Undefeatable) {
			gameCharacter.StateReset ();
		}
	}

	public override void Act() {
		base.Act ();

		gameState.PlayHitSound ();

		// project ray to front to capture object
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if(Physics.Raycast(ray, out hit, 100f))
		{
			GameObject collider = hit.collider.gameObject;

			if (collider.tag == _miniGameBombTag) {
				hit.collider.gameObject.SetActive (false);
				gameState.PlayHitFX (hit.collider.gameObject.transform.position);
				gameState.PlayPerfectSound ();
				gameState.StageClear ();
			} else {
				gameState.GameOver ();
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
