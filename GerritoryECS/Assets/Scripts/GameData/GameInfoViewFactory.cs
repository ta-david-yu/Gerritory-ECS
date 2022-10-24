using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfoViewFactory", menuName = "GameData/GameInfoViewFactory")]
public class GameInfoViewFactory : ScriptableObject, IGameInfoViewFactory
{
	[SerializeField]
	private EntityCreationEventController m_GameInfoViewControllerPrefab;

	public IEntityCreationEventController CreateGameInfoViewController()
	{
		var gameInfoViewController = GameObject.Instantiate(m_GameInfoViewControllerPrefab);
		return gameInfoViewController;
	}
}
