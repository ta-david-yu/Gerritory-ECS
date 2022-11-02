//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using JCMG.EntitasRedux;

public partial class CommandEntity
{
	/// <summary>
	/// Copies <paramref name="component"/> to this entity as a new component instance.
	/// </summary>
	public void CopyComponentTo(IComponent component)
	{
		#if !ENTITAS_REDUX_NO_IMPL
		if (component is StepKilledByOnTileElementComponent StepKilledByOnTileElement)
		{
			CopyStepKilledByOnTileElementTo(StepKilledByOnTileElement);
		}
		else if (component is ConstructTileComponent ConstructTile)
		{
			CopyConstructTileTo(ConstructTile);
		}
		else if (component is SpawnedByGlobalSpawnerComponent SpawnedByGlobalSpawner)
		{
			CopySpawnedByGlobalSpawnerTo(SpawnedByGlobalSpawner);
		}
		else if (component is ConstructAIInputComponent ConstructAIInput)
		{
			CopyConstructAIInputTo(ConstructAIInput);
		}
		else if (component is SpawnItemComponent SpawnItem)
		{
			CopySpawnItemTo(SpawnItem);
		}
		else if (component is MarkOnTileElementDeadComponent MarkOnTileElementDead)
		{
			CopyMarkOnTileElementDeadTo(MarkOnTileElementDead);
		}
		else if (component is ConstructUserInputComponent ConstructUserInput)
		{
			CopyConstructUserInputTo(ConstructUserInput);
		}
		else if (component is ConstructPlayerComponent ConstructPlayer)
		{
			CopyConstructPlayerTo(ConstructPlayer);
		}
		#endif
	}

	/// <summary>
	/// Copies all components on this entity to <paramref name="copyToEntity"/>.
	/// </summary>
	public void CopyTo(CommandEntity copyToEntity)
	{
		for (var i = 0; i < CommandComponentsLookup.TotalComponents; ++i)
		{
			if (HasComponent(i))
			{
				if (copyToEntity.HasComponent(i))
				{
					throw new EntityAlreadyHasComponentException(
						i,
						"Cannot copy component '" +
						CommandComponentsLookup.ComponentNames[i] +
						"' to " +
						this +
						"!",
						"If replacement is intended, please call CopyTo() with `replaceExisting` set to true.");
				}

				var component = GetComponent(i);
				copyToEntity.CopyComponentTo(component);
			}
		}
	}

	/// <summary>
	/// Copies all components on this entity to <paramref name="copyToEntity"/>; if <paramref name="replaceExisting"/>
	/// is true any of the components that <paramref name="copyToEntity"/> has that this entity has will be replaced,
	/// otherwise they will be skipped.
	/// </summary>
	public void CopyTo(CommandEntity copyToEntity, bool replaceExisting)
	{
		for (var i = 0; i < CommandComponentsLookup.TotalComponents; ++i)
		{
			if (!HasComponent(i))
			{
				continue;
			}

			if (!copyToEntity.HasComponent(i) || replaceExisting)
			{
				var component = GetComponent(i);
				copyToEntity.CopyComponentTo(component);
			}
		}
	}

	/// <summary>
	/// Copies components on this entity at <paramref name="indices"/> in the <see cref="CommandComponentsLookup"/> to
	/// <paramref name="copyToEntity"/>. If <paramref name="replaceExisting"/> is true any of the components that
	/// <paramref name="copyToEntity"/> has that this entity has will be replaced, otherwise they will be skipped.
	/// </summary>
	public void CopyTo(CommandEntity copyToEntity, bool replaceExisting, params int[] indices)
	{
		for (var i = 0; i < indices.Length; ++i)
		{
			var index = indices[i];

			// Validate that the index is within range of the component lookup
			if (index < 0 && index >= CommandComponentsLookup.TotalComponents)
			{
				const string OUT_OF_RANGE_WARNING =
					"Component Index [{0}] is out of range for [{1}].";

				const string HINT = "Please ensure any CopyTo indices are valid.";

				throw new IndexOutOfLookupRangeException(
					string.Format(OUT_OF_RANGE_WARNING, index, nameof(CommandComponentsLookup)),
					HINT);
			}

			if (!HasComponent(index))
			{
				continue;
			}

			if (!copyToEntity.HasComponent(index) || replaceExisting)
			{
				var component = GetComponent(index);
				copyToEntity.CopyComponentTo(component);
			}
		}
	}
}
