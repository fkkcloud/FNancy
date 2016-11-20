using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviourHelper {

	[Space(10)]
	[Header("Stage")]
	public StageDesigner StageDesignData;
	public GameObject StageObject;

	private float _currentTimeLimit;

	private Stage[] _stages;

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

	public void InitStages(){
		_stages = new Stage[StageDesignData.stageDatas.Length];

		// create all the stages : TODO : create the stages to be optimized!
		float ZPos = 0.0f;
		for (int i = 0; i < StageDesignData.stageDatas.Length; i++) 
		{
			Vector3 pos = new Vector3 (0f, 0f, ZPos);
			_stages [i] = new Stage ();
			_stages [i].Init (StageDesignData.stageDatas [i], ref StageObject, pos);
			_stages [i].stageObj.SetActive (false);
			ZPos += gameDesignVariables.StageLength;
		}

		// set current stage
		currentStageID = 0;
		currentStage.Activate ();
		Invoke ("TimerOnCallBack", gameDesignVariables.StageMoveDuration * 1.025f);

		if (_stages.Length > 0) IsReady = true;
	}

	public void GoToNextStage(){
		// deactivate current stage;
		currentStage.DeActivate ();
		StartCoroutine (StaticUtils.Hide(currentStage.stageObj, gameDesignVariables.StageMoveDuration * 1.5f));

		gameState.state = GameState.CurrentState.Moving;

		// stage clear point
		if (currentStageID == _stages.Length-1) {
			gameState.Clear ();
			return;
		}

		// currentStageID point to next stage
		currentStageID++;

		// activate next stage
		currentStage.Activate();
		Invoke ("TimerOnCallBack", gameDesignVariables.StageMoveDuration * 1.025f);

		// move all the stages
		MoveStages();
	}

	void TimerOnCallBack()
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
