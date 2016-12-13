using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticUtils : MonoBehaviour {

	// create stage objects from json
	public static float Temp1(){
		return 1f;
	}

	public static float Remap (float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	public static IEnumerator Shake(float magnitude, float duration) {

		float elapsed = 0.0f;

		Vector3 originalCamPos = Camera.main.transform.position;

		while (elapsed < duration) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			Camera.main.transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

			yield return null;
		}

		Camera.main.transform.position = originalCamPos;
	}

	public static IEnumerator AnimateFeedBackText(RectTransform rect, float duration, float from, float to, float y) {

		float elapsed = 0.0f;

		Vector3 originalCamPos = Camera.main.transform.position;

		while (elapsed < duration) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / duration;         

			float xPos = Remap (percentComplete, 0f, 1f, from, to);
			rect.anchoredPosition = new Vector2 (xPos, y);

			yield return null;
		}

		rect.anchoredPosition = new Vector2 (from, y);
	}

	public static IEnumerator Hide(GameObject obj, float time){

		yield return new WaitForSeconds(time);

		obj.SetActive (false);
	}
}
