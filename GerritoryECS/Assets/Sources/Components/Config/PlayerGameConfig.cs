using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class PlayerGameConfig
{
	public int PlayerId;
	public string PlayerName;
	public int ColorId;
	public int SkinId;

	public bool IsAI = false;
}
