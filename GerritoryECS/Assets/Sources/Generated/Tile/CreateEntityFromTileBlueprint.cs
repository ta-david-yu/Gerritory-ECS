//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using JCMG.EntitasRedux;

/// <summary>
/// Create an entity from the given <see cref="TileEntity"/> blueprint component and link it to this gameObject
/// </summary>
[RequireComponent(typeof(TileBlueprintBehaviour))]
public class CreateEntityFromTileBlueprint : MonoBehaviour
{
	[System.Serializable]
	public class ExecuteEvents
	{
		[Tooltip("Invoked when the entity is created")]
		public UnityEvent<IEntity> OnEntityCreated;

		[Tooltip("Invoked when the given blueprint is applied to the created entity")]
		public UnityEvent<IEntity> OnBlueprintApplied;
	}

	public enum ExecutionTime
	{
		Never,
		Awake,
		Start,
	}

	[SerializeField]
	private ExecutionTime m_ExecutionTime = ExecutionTime.Never;

	[SerializeField]
	private TileBlueprintBehaviour m_Blueprint;

	public ExecuteEvents Events = new ExecuteEvents();

	private EntityLink m_LinkedEntity = null;

	private void Awake()
	{
		if (m_ExecutionTime == ExecutionTime.Awake)
		{
			Execute();
		}
	}

	// Start is called before the first frame update
	private void Start()
	{
		if (m_ExecutionTime == ExecutionTime.Start)
		{
			Execute();
		}
	}

	private void OnValidate()
	{
		if (m_Blueprint == null)
		{
			m_Blueprint = GetComponent<TileBlueprintBehaviour>();
		}
	}

	public void Execute()
	{
		if (m_LinkedEntity != null)
		{
			Debug.LogWarning($"There is already an entity ({m_LinkedEntity.Entity.CreationIndex}) created from this behaveiour attached to the gameObject ${gameObject.name}.");
			return;
		}

		var entity = Contexts.SharedInstance.Tile.CreateEntity();
		Events.OnEntityCreated?.Invoke(entity);

		m_Blueprint.ApplyToEntity(entity);
		Events.OnBlueprintApplied?.Invoke(entity);

		m_LinkedEntity = gameObject.Link(entity);
	}
}
