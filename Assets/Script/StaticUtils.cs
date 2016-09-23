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
	public static StageData[] CreateStages(TextAsset StageJson){
		if (StageJson){
			string[] data = StageJson.text.Split ('$'); // split each stage into using $ sign

			int count = data.Length;
			StageData[] stages = new StageData[count];

			for (int i = 0; i < data.Length; i++) {

				// get each stage info
				string json = data [i];

				// create stage instance from json finally
				StageData stage = JsonUtility.FromJson<StageData> (json);
				stages [i] = stage;
			}
			return stages;
		}
		return null;
	}
}
