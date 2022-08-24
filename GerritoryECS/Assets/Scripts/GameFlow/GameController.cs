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
	
	private static Systems createSystems(Contexts contexts)
	{
		return new Feature("Systems")
			.Add(new MovementFeature(contexts))
			.Add(new InputFeature(contexts));
	}

	private static Systems createDebugSystems(Contexts contexts)
	{
		return new Feature("Debug Systems")
			.Add(new MovementDebugFeature(contexts));
	}

	// Start is called before the first frame update
	private void Start()
	{
		var contexts = Contexts.SharedInstance;

		for (int i = 0; i < 4; i++)
		{
			// Create player game entity
			GameEntity playerEntity = contexts.Game.CreateEntity();
			playerEntity.AddPlayer(i);
			playerEntity.AddOnTileElement(Vector2Int.zero);

			// Create user input entity
			InputEntity userInputEntity = contexts.Input.CreateEntity();
			userInputEntity.AddUserInput(i, i);
		}

		// Create player game entity
		GameEntity aiPlayerEntity = contexts.Game.CreateEntity();
		aiPlayerEntity.AddPlayer(5);
		aiPlayerEntity.AddOnTileElement(Vector2Int.right);

		// Create user input entity
		InputEntity aiInputEntity = contexts.Input.CreateEntity();
		aiInputEntity.AddAIInput(Movement.Type.Down, 5);

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

	private void OnGUI()
	{
		m_DebugSystems.Update();
	}
}
