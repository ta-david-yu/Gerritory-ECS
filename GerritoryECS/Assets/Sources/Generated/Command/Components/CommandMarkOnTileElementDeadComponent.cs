//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class CommandEntity
{
	public MarkOnTileElementDeadComponent MarkOnTileElementDead { get { return (MarkOnTileElementDeadComponent)GetComponent(CommandComponentsLookup.MarkOnTileElementDead); } }
	public bool HasMarkOnTileElementDead { get { return HasComponent(CommandComponentsLookup.MarkOnTileElementDead); } }

	public void AddMarkOnTileElementDead(int newTargetOnTileElementId)
	{
		var index = CommandComponentsLookup.MarkOnTileElementDead;
		var component = (MarkOnTileElementDeadComponent)CreateComponent(index, typeof(MarkOnTileElementDeadComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.TargetOnTileElementId = newTargetOnTileElementId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMarkOnTileElementDead(int newTargetOnTileElementId)
	{
		var index = CommandComponentsLookup.MarkOnTileElementDead;
		var component = (MarkOnTileElementDeadComponent)CreateComponent(index, typeof(MarkOnTileElementDeadComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.TargetOnTileElementId = newTargetOnTileElementId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMarkOnTileElementDeadTo(MarkOnTileElementDeadComponent copyComponent)
	{
		var index = CommandComponentsLookup.MarkOnTileElementDead;
		var component = (MarkOnTileElementDeadComponent)CreateComponent(index, typeof(MarkOnTileElementDeadComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.TargetOnTileElementId = copyComponent.TargetOnTileElementId;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMarkOnTileElementDead()
	{
		RemoveComponent(CommandComponentsLookup.MarkOnTileElementDead);
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
public sealed partial class CommandMatcher
{
	static JCMG.EntitasRedux.IMatcher<CommandEntity> _matcherMarkOnTileElementDead;

	public static JCMG.EntitasRedux.IMatcher<CommandEntity> MarkOnTileElementDead
	{
		get
		{
			if (_matcherMarkOnTileElementDead == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<CommandEntity>)JCMG.EntitasRedux.Matcher<CommandEntity>.AllOf(CommandComponentsLookup.MarkOnTileElementDead);
				matcher.ComponentNames = CommandComponentsLookup.ComponentNames;
				_matcherMarkOnTileElementDead = matcher;
			}

			return _matcherMarkOnTileElementDead;
		}
	}
}
