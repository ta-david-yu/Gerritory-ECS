using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Game Config", fileName = "GameConfig")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
	[SerializeField]
	private Vector2Int m_LevelSize = new Vector2Int(10, 10);
	public Vector2Int LevelSize => m_LevelSize;
}
