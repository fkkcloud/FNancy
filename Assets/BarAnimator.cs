using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarAnimator : MonoBehaviour {

	public GameObject Bar;
	public GameObject BarDot;

	public float YMin;
	public float YMax;

	private float[] pos;

	public float Remap (float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	// Use this for initialization
	void Start () {
		
	}

	public void Setup(int StageCount){
		pos = new float[StageCount];
		for (int i = 0; i < StageCount; i++){
			pos [i] = Remap (i, 0, StageCount - 1, YMin, YMax);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDotPosition(int id){
		LeanTween.moveLocalY(BarDot, pos[id], 0.3f);
	}
}
