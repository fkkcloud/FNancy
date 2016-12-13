using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
	public StageDesigner StageDesignData;
}

public class StageManager : MonoBehaviourHelper {

	[Tooltip ("Prefab to grab to create stage env")]
	public GameObject StageObject;

	private Stage[] _stages;

	private float _currentTimeLimit;

	public bool IsReady { get; set;}

	private int _currentStageID;
	public int currentStageID
	{
		get { 
			return _currentStageID;
		}
		set { 
			_currentStageID = value;
		}
	}

	public Stage GetCurrentStage(){
		return _stages [currentStageID];
	}

	public Stage GetPreviousStage(){
		if (currentStageID > 0)
			return _stages [currentStageID - 1];
		else
			return null;
	}

	public float GetCurrentStageTimeMult(){
		float i = (float)currentStageID / globalVariables.stageDocuments [globalVariables.SelectedLevel].StageDesignData.stageDatas.Length;
		return globalVariables.stageDocuments[globalVariables.SelectedLevel].StageDesignData.speedCurve.Evaluate(i);
	}

	public void DestroyStages(){
		for (int i = 0; i < _stages.Length; i++) {
			_stages [i].DestroyStageObject ();
			DestroyImmediate (_stages[i]);
		}
	}

	public void InitStages(){
		gameState.state = GameState.CurrentState.Playing;

		int selectedLevel = globalVariables.SelectedLevel;
		int stageCount = globalVariables.stageDocuments[selectedLevel].StageDesignData.stageDatas.Length;
		_stages = new Stage[stageCount];

		// create all the stages : TODO : create the stages to be optimized!
		float ZPos = 0.0f;
		for (int i = 0; i < stageCount; i++) 
		{
			Vector3 pos = new Vector3 (0f, 0f, ZPos);
			_stages [i] = new Stage ();
			_stages [i].Init (globalVariables.stageDocuments[selectedLevel].StageDesignData.stageDatas [i], ref StageObject, pos);
			ZPos += gameDesignVariables.StageLength;
		}

		// set current stage
		currentStageID = 0;
		ActivateStage ();

		if (_stages.Length > 0) IsReady = true;
	}

	public void GoToNextStage(){
		// deactivate current stage with delay
		currentStage.Close ();
		StartCoroutine (StaticUtils.Hide(currentStage.stageObj, gameDesignVariables.StageMoveDuration * 1.5f));

		gameState.state = GameState.CurrentState.Moving;

		// check level CLEAR point - show summary
		if (currentStageID == _stages.Length-1) {
			gameState.Clear ();
			return;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// currentStageID point to next stage ////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		currentStageID++;
		ActivateStage ();

		// move all the stages
		MoveStages();
	}

	void ActivateStage(){
		currentStage.Open();
		Invoke ("TimerOnDelay", gameDesignVariables.StageMoveDuration * 1.025f);
	}

	void TimerOnDelay()
	{
		currentStage.gameMode.StartGame ();
		gameState.PlayerIdle ();
		gameState.state = GameState.CurrentState.Playing;
		Debug.Log ("CurrentStage:" + (currentStageID + 1));
	}

	void MoveStages(){
		float ZPos = gameDesignVariables.StageLength;
		for (int i = 0; i < _stages.Length; i++) {
			GameObject CurrStage = _stages [i].stageObj;
			float newZPos = CurrStage.transform.position.z - ZPos;
			LeanTween.moveZ (CurrStage, newZPos, gameDesignVariables.StageMoveDuration).setEase (LeanTweenType.easeInQuad);
		}
	}
}
