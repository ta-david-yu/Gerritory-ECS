//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ElementEntity
{
	static readonly LeaveStateComponent LeaveStateComponent = new LeaveStateComponent();

	public bool IsLeaveState
	{
		get { return HasComponent(ElementComponentsLookup.LeaveState); }
		set
		{
			if (value != IsLeaveState)
			{
				var index = ElementComponentsLookup.LeaveState;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: LeaveStateComponent;

					AddComponent(index, component);
				}
				else
				{
					RemoveComponent(index);
				}
			}
		}
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
public sealed partial class ElementMatcher
{
	static JCMG.EntitasRedux.IMatcher<ElementEntity> _matcherLeaveState;

	public static JCMG.EntitasRedux.IMatcher<ElementEntity> LeaveState
	{
		get
		{
			if (_matcherLeaveState == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ElementEntity>)JCMG.EntitasRedux.Matcher<ElementEntity>.AllOf(ElementComponentsLookup.LeaveState);
				matcher.ComponentNames = ElementComponentsLookup.ComponentNames;
				_matcherLeaveState = matcher;
			}

			return _matcherLeaveState;
		}
	}
}
