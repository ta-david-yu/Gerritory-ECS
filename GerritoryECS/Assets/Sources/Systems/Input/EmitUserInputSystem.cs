using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;

/// <summary>
/// <see cref="EmitUserInputSystem"/> emits Unity Input to PlayerInputComponent.
/// Ideally we want to separate this into multiple systems that handle different types of input (Movement, UseItem etc).
/// For now <see cref="EmitUserInputSystem"/> handles all inputs from Player.
/// </summary>
public sealed class EmitUserInputSystem : IInitializeSystem, IUpdateSystem, ITearDownSystem
{
	private readonly GameContext m_GameContext;
	private readonly InputContext m_InputContext;
	private readonly IGroup<InputEntity> m_UserInputGroup;
	private List<InputManager> m_PlayerInputManagers;
	private const float k_DecayTime = 0.15f;

	public EmitUserInputSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_InputContext = contexts.Input;

		m_UserInputGroup = m_InputContext.GetGroup(InputMatcher.AllOf(InputMatcher.UserInput));
	}

	public void Initialize()
	{
		m_PlayerInputManagers = new List<InputManager>();
		foreach (var inputEntity in m_UserInputGroup)
		{
			var inputManager = new InputManager();
			var userInput = InputUser.PerformPairingWithDevice(UnityEngine.InputSystem.InputSystem.GetDevice<Keyboard>());
			userInput.AssociateActionsWithUser(inputManager);
			userInput.ActivateControlScheme("Normal");
			inputManager.Enable();

			m_PlayerInputManagers.Add(inputManager);
		}
	}

	public void Update()
	{
		// TODO: loop through all user input, then apply input aciton to the target player entity
		foreach (var entity in m_UserInputGroup)
		{
			int targetPlayerId = entity.UserInput.TargetPlayerId;
			GameEntity playerEntity = m_GameContext.GetEntityWithPlayer(targetPlayerId);

			Vector2 axis = m_PlayerInputManagers[entity.UserInput.UserIndex].Player.Move.ReadValue<Vector2>();

			if (axis.x > 0.5f)
			{
				playerEntity.ReplaceMovementInputAction(Movement.Type.Right, k_DecayTime);
			}
			else if (axis.y < -0.5f)
			{
				playerEntity.ReplaceMovementInputAction(Movement.Type.Down, k_DecayTime);
			}
			else if (axis.x < -0.5f)
			{
				playerEntity.ReplaceMovementInputAction(Movement.Type.Left, k_DecayTime);
			}
			else if (axis.y > 0.5f)
			{
				playerEntity.ReplaceMovementInputAction(Movement.Type.Up, k_DecayTime);
			}
		}
	}

	public void TearDown()
	{
		foreach (var inputManager in m_PlayerInputManagers)
		{
			inputManager.Disable();
		}
	}
}
