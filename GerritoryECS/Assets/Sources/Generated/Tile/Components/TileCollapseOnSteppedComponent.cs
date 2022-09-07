//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class TileEntity
{
	public CollapseOnSteppedComponent CollapseOnStepped { get { return (CollapseOnSteppedComponent)GetComponent(TileComponentsLookup.CollapseOnStepped); } }
	public bool HasCollapseOnStepped { get { return HasComponent(TileComponentsLookup.CollapseOnStepped); } }

	public void AddCollapseOnStepped(int newNumberOfStepsLeft)
	{
		var index = TileComponentsLookup.CollapseOnStepped;
		var component = (CollapseOnSteppedComponent)CreateComponent(index, typeof(CollapseOnSteppedComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.NumberOfStepsLeft = newNumberOfStepsLeft;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceCollapseOnStepped(int newNumberOfStepsLeft)
	{
		var index = TileComponentsLookup.CollapseOnStepped;
		var component = (CollapseOnSteppedComponent)CreateComponent(index, typeof(CollapseOnSteppedComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.NumberOfStepsLeft = newNumberOfStepsLeft;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyCollapseOnSteppedTo(CollapseOnSteppedComponent copyComponent)
	{
		var index = TileComponentsLookup.CollapseOnStepped;
		var component = (CollapseOnSteppedComponent)CreateComponent(index, typeof(CollapseOnSteppedComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.NumberOfStepsLeft = copyComponent.NumberOfStepsLeft;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveCollapseOnStepped()
	{
		RemoveComponent(TileComponentsLookup.CollapseOnStepped);
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
public sealed partial class TileMatcher
{
	static JCMG.EntitasRedux.IMatcher<TileEntity> _matcherCollapseOnStepped;

	public static JCMG.EntitasRedux.IMatcher<TileEntity> CollapseOnStepped
	{
		get
		{
			if (_matcherCollapseOnStepped == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TileEntity>)JCMG.EntitasRedux.Matcher<TileEntity>.AllOf(TileComponentsLookup.CollapseOnStepped);
				matcher.ComponentNames = TileComponentsLookup.ComponentNames;
				_matcherCollapseOnStepped = matcher;
			}

			return _matcherCollapseOnStepped;
		}
	}
}