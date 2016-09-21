using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticUtils : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	// create stage objects from json
	public static Stage[] CreateStages(TextAsset StageJson){
		if (StageJson){
			string[] data = StageJson.text.Split ('$');

			int count = data.Length;
			Stage[] stages = new Stage[count];

			for (int i = 0; i < data.Length; i++) {
				string json = data [i];
				Stage stage = JsonUtility.FromJson<Stage> (json);	
				stages [i] = stage;
			}
			return stages;
		}
		return null;
	}
}
