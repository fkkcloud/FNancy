using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StageData
{
	public int level; // current level
	public float[] layers;
	public float [] reward;
	public int gamemode;
	public int hp;

}

[CreateAssetMenu(fileName = "Data", menuName = "GameDesign/StageDesigner", order = 1)]
public class StageDesigner : ScriptableObject
{
	public StageData[] stageDatas;
}
