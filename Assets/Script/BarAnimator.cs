using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarAnimator : MonoBehaviourHelper {

	public GameObject Bar;
	public GameObject BarDot;
	public GameObject BarDot2;

	public float YMin = 0.5f;
	public float YMax = 1.5f;

	private float[] pos;

	public float Remap (float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	// Use this for initialization
	void Start () {
		
	}

	public void Setup(StageData[] stageDatas){
		int stageCount = stageDatas.Length;
		pos = new float[stageCount];
		for (int i = 0; i < stageCount; i++){
			pos [i] = Remap (i, 0, stageCount - 1, YMin, YMax);
			if (stageDatas [i].gamemode != 0) {
				GameObject cube = Instantiate(BarDot2);
				cube.transform.parent = Bar.transform;
				Vector3 barRelative = Bar.transform.TransformPoint (cube.transform.position);
				cube.transform.position = new Vector3(barRelative.x, pos [i], barRelative.z);
			}
		}

		gameObject.transform.Rotate(12f, 0f, 2.75f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDotPosition(){
		int id = stageManager.currentStageID;
		LeanTween.moveLocalY(BarDot, pos[id], 0.3f);
	}
}
