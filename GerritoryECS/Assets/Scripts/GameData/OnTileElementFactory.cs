using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnTileElementFactory", menuName = "GameData/OnTileElementFactory")]
public class OnTileElementFactory : ScriptableObject, IOnTileElementFactory
{
	[SerializeField]
	private EntityCreationEventController m_PlayerPrefab;

	[SerializeField]
	private EntityCreationEventController m_GhostPrefab;

	private GameObject m_CreatedViewRoot;
	public GameObject CreatedViewRoot
	{
		get
		{
			if (m_CreatedViewRoot == null)
			{
				m_CreatedViewRoot = new GameObject("OnTileElementViewsRoot");
			}

			return m_CreatedViewRoot;
		}
	}

	public IEntityCreationEventController CreatePlayerView(int playerId, int teamId, int skinId)
	{
		var playerUnityView = GameObject.Instantiate(m_PlayerPrefab, CreatedViewRoot.transform);
		return playerUnityView;
	}

	public IEntityCreationEventController CreateGhostView()
	{
		var ghostUnityView = GameObject.Instantiate(m_GhostPrefab, CreatedViewRoot.transform);
		return ghostUnityView;
	}
}
