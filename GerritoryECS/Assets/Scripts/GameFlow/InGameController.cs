using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

/// <summary>
/// <see cref="InGameController"/> is used to connect Unity Engine lifecycle game events with Entitas system events
/// </summary>
public class InGameController : MonoBehaviour
{
	[SerializeField]
	private ScriptableGameConfig m_GameConfig;

	private	Systems m_Systems;
	private Systems m_OnGUIDebugSystems;
	private Systems m_OnDrawGizmosDebugSystems;

	private Contexts m_CacheContexts;

	// Start is called before the first frame update
	private void Awake()
	{
		var contexts = Contexts.SharedInstance;

		// Initialize systems
		m_Systems = createSystems(contexts);
		m_OnGUIDebugSystems = createOnGUIDebugSystems(contexts);
		m_OnDrawGizmosDebugSystems = createOnDrawGizmosDebugSystems(contexts);

		// Setup game config for systems to initialize
		contexts.Config.SetGameConfig(m_GameConfig);

		m_CacheContexts = contexts;
	}

	private void Start()
	{
		m_Systems.Initialize();
		m_OnGUIDebugSystems.Initialize();
	}

	private void FixedUpdate()
	{
		m_Systems.FixedUpdate();
	}

	private void Update()
	{
		m_Systems.Update();
		m_Systems.Execute();
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
			
			// Core gameplay initialization feature
			.Add(new GameInfoFeature(contexts))
			.Add(new InitializationFeature(contexts))

			// In-game-scene state machine feature, this can be removed
			.Add(new InGameFlowFeature(contexts))

			// Core gameplay features
			.Add(new MovementFeature(contexts))
			.Add(new TileFeature(contexts))
			.Add(new ItemFeature(contexts))
			.Add(new ElementFeature(contexts))
			.Add(new PlayerStateFeature(contexts))

			.Add(new InputFeature(contexts))
			.Add(new MessageFeature(contexts))

			// Generated event systems
			.Add(new TileEventSystems(contexts))
			.Add(new ItemEventSystems(contexts))
			.Add(new ElementEventSystems(contexts))
			.Add(new PlayerStateEventSystems(contexts))
			.Add(new LevelEventSystems(contexts))

			// Generated cleanup systems
			.Add(new ElementCleanupSystems(contexts.Element))
			.Add(new TileCleanupSystems(contexts.Tile));
	}

	private static Systems createOnGUIDebugSystems(Contexts contexts)
	{
		return new Feature("OnGUI Systems")
			.Add(new OnGUIDebugFeature(contexts));
	}

	private static Systems createOnDrawGizmosDebugSystems(Contexts contexts)
	{
		return new Feature("Gizmos Systems")
			.Add(new OnDrawGizmosDebugFeature(contexts));
	}

	private void OnGUI()
	{
		m_OnGUIDebugSystems.Update();
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			return;
		}

		m_OnDrawGizmosDebugSystems.Update();
	}

	[ContextMenu("Spawn Ghost")]
	private void _spawnGhost()
	{
		m_CacheContexts.ConstructGhostEntity();
	}
}
