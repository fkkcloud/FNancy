using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "GameDesign/StageDesignerTool")]
public class StageDesigner : ScriptableObject
{
	public AnimationCurve speedCurve;
	public StageData[] stageDatas;

	public void Init(StageData[] stageDatas)
	{
		this.stageDatas = stageDatas;
	}

	public static StageDesigner CreateInstance(StageData[] stageDatas)
	{
		StageDesigner data = ScriptableObject.CreateInstance<StageDesigner>();
		data.Init(stageDatas);
		return data;
	}

}