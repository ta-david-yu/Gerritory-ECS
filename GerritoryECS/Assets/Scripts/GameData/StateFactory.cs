using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateFactory", menuName = "GameData/StateFactory")]
public class StateFactory : ScriptableObject, IStateFactory
{
	[System.Serializable]
	private struct StateType
	{
		public string Name;
		public PlayerStateBlueprint Blueprint;
		public Color Color;
	}

	[SerializeField]
	private List<StateType> m_StateTypes = new List<StateType>();

	public bool TryGetStateBlueprint(int id, out IPlayerStateBlueprint blueprint)
	{
		if (id < 0 || id >= m_StateTypes.Count)
		{
			blueprint = null;
			return false;
		}

		blueprint = m_StateTypes[id].Blueprint;
		return true;
	}

	public bool TryGetStateColor(int id, out Color color)
	{
		if (id < 0 || id >= m_StateTypes.Count)
		{
			color = Color.cyan;
			return false;
		}

		color = m_StateTypes[id].Color;
		return true;
	}

	public bool TryGetStateName(int id, out string name)
	{
		if (id < 0 || id >= m_StateTypes.Count)
		{
			name = "Unnamed State";
			return false;
		}

		name = m_StateTypes[id].Name;
		return true;
	}
}
