using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stage
{
	public int level = 1; // current level
	public float time = 1.0f; // json controlled for balance
	public float reward = 0.0f;
}
