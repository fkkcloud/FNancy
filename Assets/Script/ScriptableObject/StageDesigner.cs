using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "StageDesigner", menuName = "GameDesign/StageDesignerTool", order = 1)]
public class StageDesigner : ScriptableObject
{
	public AnimationCurve speedCurve;
	public StageData[] stageDatas;

}