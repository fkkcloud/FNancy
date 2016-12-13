using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

	[Space(10)]
	[Header("Stage")]

	public int SelectedLevel = 1;

	public LevelData[] stageDocuments;

	private static GameObject _instance;

	void Awake() {
		// have it work as singleton
		if (_instance) {
			DestroyImmediate (gameObject);
			return;
		}

		_instance = gameObject;
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
	}
}
