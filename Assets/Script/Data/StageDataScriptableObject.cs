using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "GameDesign/StageDesigner", order = 1)]
public class StageDesigner : ScriptableObject
{
	public StageData[] stageDatas;
}