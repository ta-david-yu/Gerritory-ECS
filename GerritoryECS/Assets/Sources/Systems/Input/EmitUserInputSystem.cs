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
	private readonly ElementContext m_ElementContext;
	private readonly InputContext m_InputContext;
	private readonly IGroup<InputEntity> m_UserInputGroup;
	private const float k_DecayTime = 0.15f;

	public EmitUserInputSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_InputContext = contexts.Input;

		m_UserInputGroup = m_InputContext.GetGroup(InputMatcher.AllOf(InputMatcher.UserInput));
	}

	public void Initialize()
	{
	}

	public void Update()
	{
		// TODO: loop through all user input, then apply input aciton to the target player entity
		foreach (var entity in m_UserInputGroup)
		{
			int userIndex = entity.UserInput.UserId;
			int targetPlayerId = entity.UserInput.TargetPlayerId;
			ElementEntity playerEntity = m_ElementContext.GetEntityWithPlayer(targetPlayerId);

			if (playerEntity == null)
			{
				// Couldn't find a player entity with the given id. Skip the input.
				Debug.LogWarning($"Couldn't find a player entity with the given id - {targetPlayerId}. The input is ignored.");
				continue;
			}

			InputActionManager userInputActions = InputManager.Instance.GetOrCreateUserInputWithIndex(userIndex);
			Vector2 axis = userInputActions.Player.Move.ReadValue<Vector2>();

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
	}
}
