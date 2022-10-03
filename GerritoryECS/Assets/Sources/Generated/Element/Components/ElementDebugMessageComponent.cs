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
	public DebugMessageComponent DebugMessage { get { return (DebugMessageComponent)GetComponent(ElementComponentsLookup.DebugMessage); } }
	public bool HasDebugMessage { get { return HasComponent(ElementComponentsLookup.DebugMessage); } }

	public void AddDebugMessage(string newMessage)
	{
		var index = ElementComponentsLookup.DebugMessage;
		var component = (DebugMessageComponent)CreateComponent(index, typeof(DebugMessageComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Message = newMessage;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceDebugMessage(string newMessage)
	{
		var index = ElementComponentsLookup.DebugMessage;
		var component = (DebugMessageComponent)CreateComponent(index, typeof(DebugMessageComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Message = newMessage;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyDebugMessageTo(DebugMessageComponent copyComponent)
	{
		var index = ElementComponentsLookup.DebugMessage;
		var component = (DebugMessageComponent)CreateComponent(index, typeof(DebugMessageComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Message = copyComponent.Message;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveDebugMessage()
	{
		RemoveComponent(ElementComponentsLookup.DebugMessage);
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
public partial class ElementEntity : IDebugMessageEntity { }

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
	static JCMG.EntitasRedux.IMatcher<ElementEntity> _matcherDebugMessage;

	public static JCMG.EntitasRedux.IMatcher<ElementEntity> DebugMessage
	{
		get
		{
			if (_matcherDebugMessage == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ElementEntity>)JCMG.EntitasRedux.Matcher<ElementEntity>.AllOf(ElementComponentsLookup.DebugMessage);
				matcher.ComponentNames = ElementComponentsLookup.ComponentNames;
				_matcherDebugMessage = matcher;
			}

			return _matcherDebugMessage;
		}
	}
}