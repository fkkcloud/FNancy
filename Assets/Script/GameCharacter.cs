using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour {

	public GameObject CharacterObj;
	public int perfectMax = 2;
	public int undefeatMax = 3;
	public int currentUndefeatCount = 0;
	public int currentPerfect = 0;

	public GameObject HighlightFX; // y = 0.64 ~ 0.82

	public enum CurrentState {Normal, Undefeatable, Tired, Drunken};
	public CurrentState _state = CurrentState.Normal;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StateReset(){
		_state = CurrentState.Normal;
		currentPerfect = 0;
		currentUndefeatCount = 0;

		HighlightFX.SetActive (false); 
	}

	public float Remap (float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	public void UpdateCharacterState(){
		if (currentUndefeatCount == undefeatMax) {
			StateReset ();
		}
		if (currentPerfect == perfectMax) {
			_state = CurrentState.Undefeatable;
		}
		if (currentPerfect > 0) {
			HighlightFX.SetActive (true);
			float size = Remap (currentPerfect, 1, perfectMax, 0.5f, 2f);
			HighlightFX.transform.localScale = new Vector3 (size, size, size);
		}
	}

	public void Play(string animation){
		CharacterObj.GetComponent<Animation> ().Play (animation);	
	}

	public void Deactivate(){
		HighlightFX.SetActive (false);
	}
}
