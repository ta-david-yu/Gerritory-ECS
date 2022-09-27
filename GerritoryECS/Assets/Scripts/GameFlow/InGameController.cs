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
			.Add(new InputFeature(contexts))

			.Add(new LevelFeature(contexts))
			.Add(new MovementFeature(contexts))
			.Add(new TileFeature(contexts))
			.Add(new ItemFeature(contexts))
			.Add(new PlayerStateFeature(contexts))

			.Add(new MessageFeature(contexts))

			.Add(new GameEventSystems(contexts))
			.Add(new TileEventSystems(contexts))

			.Add(new GameCleanupSystems(contexts.Game))
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
}
