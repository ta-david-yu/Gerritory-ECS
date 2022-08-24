using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

/// <summary>
/// <see cref="EmitUserInputSystem"/> emits Unity Input to PlayerInputComponent.
/// Ideally we want to separate this into multiple systems that handle different types of input (Movement, UseItem etc).
/// For now <see cref="EmitUserInputSystem"/> handles all inputs from Player.
/// </summary>
public sealed class EmitUserInputSystem : IInitializeSystem, IUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly InputContext m_InputContext;
	private readonly IGroup<InputEntity> m_UserInputGroup;

	private const float k_DecayTime = 0.05f;

	public EmitUserInputSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
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
			int targetPlayerId = entity.UserInput.TargetPlayerId;
			GameEntity playerEntity = m_GameContext.GetEntityWithPlayer(targetPlayerId);

			if (Input.GetKey(KeyCode.D))
			{
				playerEntity.ReplaceMovementInputAction(Movement.Type.Right);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				playerEntity.ReplaceMovementInputAction(Movement.Type.Down);
			}
			else if (Input.GetKey(KeyCode.A))
			{
				playerEntity.ReplaceMovementInputAction(Movement.Type.Left);
			}
			else if (Input.GetKey(KeyCode.W))
			{
				playerEntity.ReplaceMovementInputAction(Movement.Type.Up);
			}
		}
	}
}
