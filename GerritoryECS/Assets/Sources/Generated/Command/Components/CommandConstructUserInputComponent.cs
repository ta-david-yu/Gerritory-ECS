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
	public ConstructUserInputComponent ConstructUserInput { get { return (ConstructUserInputComponent)GetComponent(CommandComponentsLookup.ConstructUserInput); } }
	public bool HasConstructUserInput { get { return HasComponent(CommandComponentsLookup.ConstructUserInput); } }

	public void AddConstructUserInput(int newUserId, int newTargetElementId)
	{
		var index = CommandComponentsLookup.ConstructUserInput;
		var component = (ConstructUserInputComponent)CreateComponent(index, typeof(ConstructUserInputComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.UserId = newUserId;
		component.TargetElementId = newTargetElementId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceConstructUserInput(int newUserId, int newTargetElementId)
	{
		var index = CommandComponentsLookup.ConstructUserInput;
		var component = (ConstructUserInputComponent)CreateComponent(index, typeof(ConstructUserInputComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.UserId = newUserId;
		component.TargetElementId = newTargetElementId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyConstructUserInputTo(ConstructUserInputComponent copyComponent)
	{
		var index = CommandComponentsLookup.ConstructUserInput;
		var component = (ConstructUserInputComponent)CreateComponent(index, typeof(ConstructUserInputComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.UserId = copyComponent.UserId;
		component.TargetElementId = copyComponent.TargetElementId;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveConstructUserInput()
	{
		RemoveComponent(CommandComponentsLookup.ConstructUserInput);
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
	static JCMG.EntitasRedux.IMatcher<CommandEntity> _matcherConstructUserInput;

	public static JCMG.EntitasRedux.IMatcher<CommandEntity> ConstructUserInput
	{
		get
		{
			if (_matcherConstructUserInput == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<CommandEntity>)JCMG.EntitasRedux.Matcher<CommandEntity>.AllOf(CommandComponentsLookup.ConstructUserInput);
				matcher.ComponentNames = CommandComponentsLookup.ComponentNames;
				_matcherConstructUserInput = matcher;
			}

			return _matcherConstructUserInput;
		}
	}
}
