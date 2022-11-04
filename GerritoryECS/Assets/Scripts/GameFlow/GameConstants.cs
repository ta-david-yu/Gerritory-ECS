using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
	public const float TileOffset = 1;
	public static readonly Quaternion InGameCameraOrientation = new Quaternion(-0.287274003f, -0.227902189f, 0.0705751702f, -0.927660227f);
	public const float MoveOnTileDuration = 0.33f;
	public const float WaitingForRespawnDuration = 3.0f;
	public const float CountdownTime = 3.0f;
	public const float UserHoldInputDecayTime = 0.08f;
	public const float UserTapInputDecayTime = 0.3f;

	/// <summary>
	/// When the game timer is bigger than the value, the LeadingTeam system would start updating the LeadingTeam info based on TeamGameRankings.
	/// </summary>
	public const float HasLeadingTeamTime = 5.0f;
	public const int MaxTeamCount = 32;

	public const int MaxGhostChaseVictimHeuristicDistance = 20;
	public const float IgnoredByGhostOnRespawnTime = 1.5f;
	public const float GhostBaseSpeed = 1.15f;
	public const float GhostAppearingTime = 2f;
	public const float GhostDisappearingTime = 1.5f;

	public static Vector3 TilePositionToWorldPosition(Vector2Int tilePosition)
	{
		return new Vector3(tilePosition.x, 0, tilePosition.y) * GameConstants.TileOffset;
	}
}
