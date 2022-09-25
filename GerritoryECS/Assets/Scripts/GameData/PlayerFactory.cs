using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFactory", menuName = "GameData/PlayerFactory")]
public class PlayerFactory : ScriptableObject, IPlayerFactory
{
	[SerializeField]
	private EntityCreationEventController m_PlayerPrefab;

	private GameObject m_CreatedViewRoot;
	public GameObject CreatedViewRoot
	{
		get
		{
			if (m_CreatedViewRoot == null)
			{
				m_CreatedViewRoot = new GameObject("PlayerViewsRoot");
			}

			return m_CreatedViewRoot;
		}
	}

	public IEntityCreationEventController CreatePlayerView(int playerId, int colorId, int skinId)
	{
		var playerUnityView = GameObject.Instantiate(m_PlayerPrefab, CreatedViewRoot.transform);
		return playerUnityView;
	}
}
