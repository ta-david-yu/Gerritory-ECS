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

	// Start is called before the first frame update
	private void Start()
	{
		var contexts = Contexts.SharedInstance;

		m_Systems = new Feature("Systems")
			.Add(new MovementFeature(contexts))
			.Add(new InputFeature(contexts));

		m_Systems.Initialize();

		// Create player game entity
		GameEntity mainPlayerEntity = contexts.Game.CreateEntity();
		mainPlayerEntity.AddPlayer(0);
		mainPlayerEntity.AddOnTileElement(Vector2Int.zero);

		// Create user input entity
		InputEntity mainUserInputEntity = contexts.Input.CreateEntity();
		mainUserInputEntity.AddUserInput(0, 0);
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
}
