//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemEntity
{
	public ApplySpeedChangeStateOnEatenComponent ApplySpeedChangeStateOnEaten { get { return (ApplySpeedChangeStateOnEatenComponent)GetComponent(ItemComponentsLookup.ApplySpeedChangeStateOnEaten); } }
	public bool HasApplySpeedChangeStateOnEaten { get { return HasComponent(ItemComponentsLookup.ApplySpeedChangeStateOnEaten); } }

	public void AddApplySpeedChangeStateOnEaten(float newDuration, float newSpeedMultiplier)
	{
		var index = ItemComponentsLookup.ApplySpeedChangeStateOnEaten;
		var component = (ApplySpeedChangeStateOnEatenComponent)CreateComponent(index, typeof(ApplySpeedChangeStateOnEatenComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Duration = newDuration;
		component.SpeedMultiplier = newSpeedMultiplier;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceApplySpeedChangeStateOnEaten(float newDuration, float newSpeedMultiplier)
	{
		var index = ItemComponentsLookup.ApplySpeedChangeStateOnEaten;
		var component = (ApplySpeedChangeStateOnEatenComponent)CreateComponent(index, typeof(ApplySpeedChangeStateOnEatenComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Duration = newDuration;
		component.SpeedMultiplier = newSpeedMultiplier;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyApplySpeedChangeStateOnEatenTo(ApplySpeedChangeStateOnEatenComponent copyComponent)
	{
		var index = ItemComponentsLookup.ApplySpeedChangeStateOnEaten;
		var component = (ApplySpeedChangeStateOnEatenComponent)CreateComponent(index, typeof(ApplySpeedChangeStateOnEatenComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Duration = copyComponent.Duration;
		component.SpeedMultiplier = copyComponent.SpeedMultiplier;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveApplySpeedChangeStateOnEaten()
	{
		RemoveComponent(ItemComponentsLookup.ApplySpeedChangeStateOnEaten);
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class ItemMatcher
{
	static JCMG.EntitasRedux.IMatcher<ItemEntity> _matcherApplySpeedChangeStateOnEaten;

	public static JCMG.EntitasRedux.IMatcher<ItemEntity> ApplySpeedChangeStateOnEaten
	{
		get
		{
			if (_matcherApplySpeedChangeStateOnEaten == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ItemEntity>)JCMG.EntitasRedux.Matcher<ItemEntity>.AllOf(ItemComponentsLookup.ApplySpeedChangeStateOnEaten);
				matcher.ComponentNames = ItemComponentsLookup.ComponentNames;
				_matcherApplySpeedChangeStateOnEaten = matcher;
			}

			return _matcherApplySpeedChangeStateOnEaten;
		}
	}
}
