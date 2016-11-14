using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
	public StageData _stageData;

	private float _life;

	public int CurrentLayerID;

	public GameObject StageObj;

	public TextMesh TextMeshTimerIndicator;
	public TextMesh TextMeshTimer;

	private TextMesh[] _textMeshes;

	public void SetStageData(StageData data){
		_stageData = data;
		_life = data.layers.Length;
	}

	public bool IsClear()
	{
		return CurrentLayerID == _life - 1;
	}

	public void InitStageObj(ref GameObject StageObject, Vector3 pos){
		StageObj = Instantiate (StageObject);
		StageObj.transform.position = pos;

		// reset 3d text meshes to none
		TextMesh[] childrens = StageObj.GetComponentsInChildren<TextMesh>();
		foreach (TextMesh child in childrens) {
			// do what you want with the transform
			if (child.gameObject.tag == "TimerBomb") {
				child.text = "";
			} else if (child.gameObject.tag == "TimerStage") {
				child.text = "";
			}
		}

		_textMeshes = StageObj.GetComponentsInChildren<TextMesh>();

		CurrentLayerID = 0;
	}

	public float GetCurrentLayerTime()
	{
		return _stageData.layers [CurrentLayerID];
	}

	public void UpdateIndicator()
	{
		TextMeshTimerIndicator.text = _stageData.layers[CurrentLayerID].ToString();
	}
		
	public void InitText()
	{
		foreach (TextMesh child in _textMeshes) {
			// do what you want with the transform
			if (child.gameObject.tag == "TimerBomb") {
				child.text = 0.0f.ToString("#.#");
				child.gameObject.SetActive (true);
				TextMeshTimer = child;
			} else if (child.gameObject.tag == "TimerStage") {
				child.text = _stageData.layers[CurrentLayerID].ToString();
				child.gameObject.SetActive (true);
				TextMeshTimerIndicator = child;
			}
		}
	}

	public void HideTexts(){
		TextMeshTimer.gameObject.SetActive (false);
		TextMeshTimerIndicator.gameObject.SetActive (false);
	}
}
