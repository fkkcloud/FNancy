using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour {

	public GameObject CharacterObj;
	public int Preprod = 2;
	public int Postprod = 4;
	public int currentPostProd = 0;
	public int currentPreProd = 0;

	public enum CurrentState {Normal, Undefeatable, Tired, Drunken};
	public CurrentState _state = CurrentState.Normal;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateCharacterState(){
		if (currentPostProd == Postprod - 1) {
			_state = CurrentState.Normal;
			currentPreProd = 0;
			currentPostProd = 0;
		}

		if (Preprod-1 == currentPreProd) {
			_state = CurrentState.Undefeatable;

		}
	}

	public void Play(string animation){
		CharacterObj.GetComponent<Animation> ().Play (animation);	
	}
}
