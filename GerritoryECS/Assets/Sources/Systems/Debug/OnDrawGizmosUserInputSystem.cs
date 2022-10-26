using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDrawGizmosUserInputSystem : IUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly InputContext m_InputContext;
	private readonly Contexts m_Contexts;
	private readonly IGroup<ElementEntity> m_InLevelUserElementGroup;

	public OnDrawGizmosUserInputSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_InputContext = contexts.Input;
		m_Contexts = contexts;

		m_InLevelUserElementGroup = m_ElementContext.GetGroup(ElementMatcher.AllOf(ElementMatcher.OnTileElement, ElementMatcher.Player, ElementMatcher.OnTilePosition));
	}


	public void Update()
	{
		foreach (var elementEntity in m_InLevelUserElementGroup.GetEntities())
		{
			var userInputEntity = m_InputContext.GetEntityWithUserInputTargetPlayerId(elementEntity.Player.Id);
			if (userInputEntity == null)
			{
				// The element is not controlled by an user input. Skip it.
				continue;
			}

			Vector2Int bufferedInputMoveToLocationBase = (elementEntity.HasMoveOnTile)? elementEntity.MoveOnTile.ToPosition : elementEntity.OnTilePosition.Value; 

			Vector3 currentWorldPosition = GameConstants.TilePositionToWorldPosition(elementEntity.OnTilePosition.Value);

			Gizmos.color = Color.yellow;
			Gizmos.DrawCube(currentWorldPosition, Vector3.one * 0.2f);

			float timeUntilNextPossibleMove = 0;

			if (elementEntity.HasMoveOnTile)
			{
				timeUntilNextPossibleMove = (1 - elementEntity.MoveOnTile.Progress) * elementEntity.GetElementEntityMoveOnTileDuration();

				Vector3 movingToWorldPosition = GameConstants.TilePositionToWorldPosition(elementEntity.MoveOnTile.ToPosition);
				Vector3 progressKnobWorldPosition = Vector3.Lerp(currentWorldPosition, movingToWorldPosition, elementEntity.MoveOnTile.Progress);
				Gizmos.DrawCube(movingToWorldPosition, Vector3.one * 0.15f);
				Gizmos.DrawCube(progressKnobWorldPosition, Vector3.one * 0.1f);
				Gizmos.DrawLine(currentWorldPosition, movingToWorldPosition);
			}

			if (elementEntity.HasMovementInputAction)
			{
				// Make it green if this input will be eventually taken in by the movement system!
				Gizmos.color = (elementEntity.MovementInputAction.DecayTimer < timeUntilNextPossibleMove)? Color.green : Color.red;

				Vector2Int nextMoveTo = bufferedInputMoveToLocationBase + Movement.TypeToOffset[(int)elementEntity.MovementInputAction.Type];
				Vector3 bufferedInputMoveToWorldPosition = GameConstants.TilePositionToWorldPosition(nextMoveTo);
				Vector3 bufferedInputMoveToBaseWorldPosition = GameConstants.TilePositionToWorldPosition(bufferedInputMoveToLocationBase);
				Gizmos.DrawWireSphere(bufferedInputMoveToWorldPosition, 0.15f);

				float decayTimeLeftRatio = elementEntity.MovementInputAction.DecayTimer / GameConstants.UserInputDecayTime;
				Gizmos.DrawSphere(bufferedInputMoveToWorldPosition, decayTimeLeftRatio * 0.15f);
				Gizmos.DrawLine(bufferedInputMoveToBaseWorldPosition, bufferedInputMoveToWorldPosition);
			}
		}
	}
}
