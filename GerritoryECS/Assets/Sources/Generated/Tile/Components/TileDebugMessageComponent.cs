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
	public DebugMessageComponent DebugMessage { get { return (DebugMessageComponent)GetComponent(TileComponentsLookup.DebugMessage); } }
	public bool HasDebugMessage { get { return HasComponent(TileComponentsLookup.DebugMessage); } }

	public void AddDebugMessage(string newMessage)
	{
		var index = TileComponentsLookup.DebugMessage;
		var component = (DebugMessageComponent)CreateComponent(index, typeof(DebugMessageComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Message = newMessage;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceDebugMessage(string newMessage)
	{
		var index = TileComponentsLookup.DebugMessage;
		var component = (DebugMessageComponent)CreateComponent(index, typeof(DebugMessageComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Message = newMessage;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyDebugMessageTo(DebugMessageComponent copyComponent)
	{
		var index = TileComponentsLookup.DebugMessage;
		var component = (DebugMessageComponent)CreateComponent(index, typeof(DebugMessageComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Message = copyComponent.Message;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveDebugMessage()
	{
		RemoveComponent(TileComponentsLookup.DebugMessage);
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
public partial class TileEntity : IDebugMessageEntity { }

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
	static JCMG.EntitasRedux.IMatcher<TileEntity> _matcherDebugMessage;

	public static JCMG.EntitasRedux.IMatcher<TileEntity> DebugMessage
	{
		get
		{
			if (_matcherDebugMessage == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TileEntity>)JCMG.EntitasRedux.Matcher<TileEntity>.AllOf(TileComponentsLookup.DebugMessage);
				matcher.ComponentNames = TileComponentsLookup.ComponentNames;
				_matcherDebugMessage = matcher;
			}

			return _matcherDebugMessage;
		}
	}
}
