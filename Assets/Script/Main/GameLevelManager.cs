﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : MonoBehaviourHelper {

	public LevelManager LevelManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LevelOpen(int number){
		globalVariables.SelectedLevel = number;
		LevelManager.LoadLevel ("02_000_Base");
	}
}
