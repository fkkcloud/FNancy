using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StageData
{
	public int level; // current level
	public float timeLimit;
	public float [] reward;
	public int gamemode;
	public int hp;
	public float speed = 1.0f;
}
