using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

/// <summary>
/// <see cref="GameController"/> is used to connect Unity Engine lifecycle game events with Entitas system events
/// </summary>
public class GameController : MonoBehaviour
{
	private	Systems m_Systems;
	private Systems m_DebugSystems;

	// Start is called before the first frame update
	private void Awake()
	{
		var contexts = Contexts.SharedInstance;

		// Initialize systems
		m_Systems = createSystems(contexts);
		m_DebugSystems = createDebugSystems(contexts);

		m_Systems.Initialize();
		m_DebugSystems.Initialize();
	}

	// Update is called once per frame
	private void Update()
	{
		m_Systems.Update();
		m_Systems.Execute();
	}

	private void FixedUpdate()
	{
		m_Systems.FixedUpdate();
	}

	private void LateUpdate()
	{
		m_Systems.LateUpdate();
		m_Systems.Cleanup();
	}

	private void OnDestroy()
	{
		m_Systems.TearDown();
	}

	private static Systems createSystems(Contexts contexts)
	{
		return new Feature("Systems")
			.Add(new InputFeature(contexts))
			.Add(new MovementFeature(contexts))
			.Add(new GameEventSystems(contexts));
	}

	private static Systems createDebugSystems(Contexts contexts)
	{
		return new Feature("Debug Systems")
			.Add(new OnGUIDebugFeature(contexts));
	}

	private void OnGUI()
	{
		m_DebugSystems.Update();
	}
}
