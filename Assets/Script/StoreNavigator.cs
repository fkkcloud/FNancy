using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreNavigator : MonoBehaviour {

	public GameObject[] StoreItemPrefabs;
	public float[] StoreItemPrices;
	public string[] StoreNames;

	GameObject[] Assets;
	public LevelManager LevelManager;

	public Text PriceUI; 
	public Text ItemName;

	public Vector3 InitialPos;

	public float MoveScale = 1f;

	public int CurrentSelected = 0;

	// Use this for initialization
	void Start () {
		Vector3 pos = new Vector3 (1000f, 1000f, 1000f);
		Assets = new GameObject[StoreItemPrefabs.Length];
		for (int i = 0; i < StoreItemPrefabs.Length; i++) {
			Assets [i] = Instantiate (StoreItemPrefabs [i], pos, Quaternion.identity);
			Assets [i].SetActive (false);
		}

		Assets [CurrentSelected].transform.position = InitialPos;
		Assets [CurrentSelected].SetActive (true);

		UpdateUI ();
	}

	void UpdateUI(){
		PriceUI.text = StoreItemPrices[CurrentSelected].ToString ();
		ItemName.text = StoreNames[CurrentSelected].ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Next(){
		Animate (-1);

		CurrentSelected += 1;
		if (CurrentSelected >= Assets.Length) {
			CurrentSelected = 0;
		}

		Debug.Log (CurrentSelected);

		Vector3 movement = Vector3.right;
		movement *= 1;
		movement *= MoveScale;
		Assets [CurrentSelected].transform.position = InitialPos + movement;
		Assets [CurrentSelected].SetActive (true);

		Animate (-1);

		UpdateUI ();
	}

	public void Prev(){
		Animate (1);

		CurrentSelected -= 1;
		if (CurrentSelected < 0) {
			CurrentSelected = Assets.Length - 1;
		}

		Debug.Log (CurrentSelected);

		Vector3 movement = Vector3.right;
		movement *= -1;
		movement *= MoveScale;
		Assets [CurrentSelected].transform.position = InitialPos + movement;
		Assets [CurrentSelected].SetActive (true);

		Animate (1);

		UpdateUI ();
	}

	public void Animate(int dir){
		Vector3 movement = Vector3.right;
		movement *= dir;
		movement *= MoveScale;

		Vector3 TargetPosition = Assets [CurrentSelected].transform.position + movement;
		LeanTween.move (Assets [CurrentSelected], TargetPosition, 0.5f).setEase(LeanTweenType.easeOutQuad);
	}

	public void GoToMain(){
		LevelManager.LoadGameLevel ();
	}
}
