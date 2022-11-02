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
	public StepKilledByOnTileElementComponent StepKilledByOnTileElement { get { return (StepKilledByOnTileElementComponent)GetComponent(CommandComponentsLookup.StepKilledByOnTileElement); } }
	public bool HasStepKilledByOnTileElement { get { return HasComponent(CommandComponentsLookup.StepKilledByOnTileElement); } }

	public void AddStepKilledByOnTileElement(int newKillerOnTileElementId)
	{
		var index = CommandComponentsLookup.StepKilledByOnTileElement;
		var component = (StepKilledByOnTileElementComponent)CreateComponent(index, typeof(StepKilledByOnTileElementComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.KillerOnTileElementId = newKillerOnTileElementId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceStepKilledByOnTileElement(int newKillerOnTileElementId)
	{
		var index = CommandComponentsLookup.StepKilledByOnTileElement;
		var component = (StepKilledByOnTileElementComponent)CreateComponent(index, typeof(StepKilledByOnTileElementComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.KillerOnTileElementId = newKillerOnTileElementId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyStepKilledByOnTileElementTo(StepKilledByOnTileElementComponent copyComponent)
	{
		var index = CommandComponentsLookup.StepKilledByOnTileElement;
		var component = (StepKilledByOnTileElementComponent)CreateComponent(index, typeof(StepKilledByOnTileElementComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.KillerOnTileElementId = copyComponent.KillerOnTileElementId;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveStepKilledByOnTileElement()
	{
		RemoveComponent(CommandComponentsLookup.StepKilledByOnTileElement);
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
public partial class CommandEntity : IStepKilledByOnTileElementEntity { }

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
	static JCMG.EntitasRedux.IMatcher<CommandEntity> _matcherStepKilledByOnTileElement;

	public static JCMG.EntitasRedux.IMatcher<CommandEntity> StepKilledByOnTileElement
	{
		get
		{
			if (_matcherStepKilledByOnTileElement == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<CommandEntity>)JCMG.EntitasRedux.Matcher<CommandEntity>.AllOf(CommandComponentsLookup.StepKilledByOnTileElement);
				matcher.ComponentNames = CommandComponentsLookup.ComponentNames;
				_matcherStepKilledByOnTileElement = matcher;
			}

			return _matcherStepKilledByOnTileElement;
		}
	}
}
