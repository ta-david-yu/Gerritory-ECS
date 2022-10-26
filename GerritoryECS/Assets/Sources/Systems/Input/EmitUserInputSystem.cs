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
	private readonly ConfigContext m_ConfigContext;
	private readonly IGroup<InputEntity> m_UserInputGroup;

	public EmitUserInputSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_InputContext = contexts.Input;
		m_ConfigContext = contexts.Config;

		m_UserInputGroup = m_InputContext.GetGroup(InputMatcher.AllOf(InputMatcher.UserInput));
	}

	public void Initialize()
	{
		// Spawn input entity so the user/AI can start controlling their player entity.
		var playerConfigs = m_ConfigContext.GameConfig.value.PlayerGameConfigs;
		foreach (var playerConfig in playerConfigs)
		{
			if (!playerConfig.IsAI)
			{
				// Create the input action manager for the user so the input system would start reading the input before countdown.
				InputManager.Instance.GetOrCreateUserInputWithIndex(playerConfig.PlayerId);
			}
		}
	}

	public void Update()
	{
		// Loop through all user inputs, then apply input aciton to the target player entity
		foreach (var inputEntity in m_UserInputGroup)
		{
			int userIndex = inputEntity.UserInput.UserId;
			int targetPlayerId = inputEntity.UserInput.TargetPlayerId;
			ElementEntity playerEntity = m_ElementContext.GetEntityWithPlayer(targetPlayerId);

			if (playerEntity == null)
			{
				// Couldn't find a player entity with the given id. Skip the input.
				Debug.LogWarning($"Couldn't find a player entity with the given id - {targetPlayerId}. The input is ignored.");
				continue;
			}

			float movementDuration = playerEntity.GetElementEntityMoveOnTileDuration();
			InputActionManager userInputActions = InputManager.Instance.GetOrCreateUserInputWithIndex(userIndex);

			// Check input holding state
			Movement.Type holdMovementInput = Movement.Type.Stay;
			if (userInputActions.Player.MoveRight.IsPressed())
			{
				holdMovementInput = Movement.Type.Right;
			}
			else if (userInputActions.Player.MoveDown.IsPressed())
			{
				holdMovementInput = Movement.Type.Down;
			}
			else if (userInputActions.Player.MoveLeft.IsPressed())
			{
				holdMovementInput = Movement.Type.Left;
			}
			else if (userInputActions.Player.MoveUp.IsPressed())
			{
				holdMovementInput = Movement.Type.Up;
			}

			// Check input tapping state
			Movement.Type tapMovementInput = Movement.Type.Stay;
			if (userInputActions.Player.MoveRight.triggered)
			{
				tapMovementInput = Movement.Type.Right;
			}
			else if (userInputActions.Player.MoveDown.triggered)
			{
				tapMovementInput = Movement.Type.Down;
			}
			else if (userInputActions.Player.MoveLeft.triggered)
			{
				tapMovementInput = Movement.Type.Left;
			}
			else if (userInputActions.Player.MoveUp.triggered)
			{
				tapMovementInput = Movement.Type.Up;
			}

			if (tapMovementInput != Movement.Type.Stay)
			{
				// Movment input created through tap has higher priority, it will always override the previous movement input action.
				float tapDecayTime = Mathf.Min(GameConstants.UserTapInputDecayTime, playerEntity.GetElementEntityMoveOnTileDuration());
				playerEntity.ReplaceMovementInputAction(tapMovementInput, tapDecayTime);
			}
			else if (!playerEntity.HasMovementInputAction && holdMovementInput != Movement.Type.Stay)
			{
				// If there is no recorded movement input action, use the movement input created through holding as the input.
				playerEntity.ReplaceMovementInputAction(holdMovementInput, GameConstants.UserHoldInputDecayTime);
			}
		}
	}

	public void TearDown()
	{
	}
}
