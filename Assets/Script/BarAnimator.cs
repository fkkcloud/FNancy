using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarAnimator : MonoBehaviourHelper {

	private float[] pos;
	private Slider slider;

	public void Setup(StageData[] stageDatas){
		slider = GetComponent<Slider> ();
		float progress = (float)stageManager.currentStageID / stageDatas.Length;
		StartCoroutine (AnimateGameProgress (slider, 0.4f, progress));
	}

	public IEnumerator AnimateGameProgress(Slider slider, float duration, float to) {

		float elapsed = 0.0f;

		while (elapsed < duration) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / duration;   

			float inprogress = to * percentComplete;

			slider.value = inprogress;

			yield return null;
		}
	}

}
