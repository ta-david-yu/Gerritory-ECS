using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerGameConfig
{
	public int PlayerId;
	public string PlayerName;

	[Tooltip("TeamId is tied to ColorId")]
	public int TeamId;
	public int SkinId;

	public bool IsAI;
}
