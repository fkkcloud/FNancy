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

		// when player does not do anything
		if (_timer > _timeLimit) {
			gameState.GameOver ();
		}

		// cancel highlight combo!
		if (gameCharacter._state == GameCharacter.CurrentState.Undefeatable) {
			gameCharacter.StateReset ();
		}
	}

	public override void ReactOnTouch() {
		base.ReactOnTouch ();

		if (!_timerOn)
			return;

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

	public override void SetupUI(){

		currentStage.bombObj.SetActive (true);

		// setup UI
		currentStage.gameUIList.Add (currentStage.UITextMeshTimerIndicator.gameObject);
		currentStage.gameUIList.Add (currentStage.UIElements.GameMode3_UI);

		currentStage.gameUIList.Add (currentStage.UIElements.MiniGameText);

		// hide original bomb
		currentStage.bombObj.SetActive(false);

		float playMatLength = currentStage.UIElements.PlayMat.transform.lossyScale.x;
		Vector3 playMatPos = currentStage.UIElements.PlayMat.transform.position;

		float playMatMostLeftX = playMatPos.x - playMatLength * 0.5f;
		float playMatMostRightX = playMatPos.x + playMatLength * 0.5f;

		float[] xs = new float[3];
		xs [0] = playMatMostLeftX;
		xs [1] = playMatPos.x;
		xs [2] = playMatMostRightX;
		for (int i = 0; i < 3; i++) {

			Vector3 pos = new Vector3 (xs[i], playMatPos.y - 0.1f, playMatPos.z);
			GameObject bomb = Instantiate (currentStage.UIElements.UI_BOMB);

			bomb.transform.position = pos;
			bomb.transform.parent = currentStage.bombObj.transform.parent; // TODO : we have to have a mini game UI root transform..
			currentStage.minigamePlayUI.Add (bomb);

			if (i == 0){
				bomb.gameObject.tag = "minigame_bomb_r";
			}
			else if (i == 1){
				bomb.gameObject.tag = "minigame_bomb_g";
			}
			else{
				bomb.gameObject.tag = "minigame_bomb_b";
			}

			// color bombs
			foreach (Renderer r in bomb.GetComponentsInChildren<Renderer>()) {
				if (i == 0){
					r.material.SetColor ("_Color", new Color (1f, 0f, 0f)); // r
				}
				else if (i == 1){
					r.material.SetColor ("_Color", new Color (0f, 1f, 0f)); // g
				}
				else{
					r.material.SetColor ("_Color", new Color (0f, 0f, 1f)); // b
				}
			}
		}
	}
}
