using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour {

	public GameObject CharacterObj;
	public int perfectMax = 2;
	public int undefeatMax = 3;
	public int currentUndefeatCount = 0;
	public int currentPerfect = 0;

	public enum CurrentState {Normal, Undefeatable, Tired, Drunken};
	public CurrentState _state = CurrentState.Normal;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StateReset(){
		currentPerfect = 0;
		currentUndefeatCount = 0;
		_state = CurrentState.Normal;
	}

	public void UpdateCharacterState(){
		if (currentUndefeatCount == undefeatMax) {
			StateReset ();
		}
		if (currentPerfect == perfectMax) {
			_state = CurrentState.Undefeatable;

		}
	}

	public void Play(string animation){
		CharacterObj.GetComponent<Animation> ().Play (animation);	
	}
}
