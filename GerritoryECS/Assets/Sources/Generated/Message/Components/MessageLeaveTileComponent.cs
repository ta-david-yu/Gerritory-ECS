//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class MessageEntity
{
	public LeaveTileComponent LeaveTile { get { return (LeaveTileComponent)GetComponent(MessageComponentsLookup.LeaveTile); } }
	public bool HasLeaveTile { get { return HasComponent(MessageComponentsLookup.LeaveTile); } }

	public void AddLeaveTile(int newOnTileElementId, UnityEngine.Vector2Int newPosition)
	{
		var index = MessageComponentsLookup.LeaveTile;
		var component = (LeaveTileComponent)CreateComponent(index, typeof(LeaveTileComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.OnTileElementId = newOnTileElementId;
		component.Position = newPosition;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceLeaveTile(int newOnTileElementId, UnityEngine.Vector2Int newPosition)
	{
		var index = MessageComponentsLookup.LeaveTile;
		var component = (LeaveTileComponent)CreateComponent(index, typeof(LeaveTileComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.OnTileElementId = newOnTileElementId;
		component.Position = newPosition;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyLeaveTileTo(LeaveTileComponent copyComponent)
	{
		var index = MessageComponentsLookup.LeaveTile;
		var component = (LeaveTileComponent)CreateComponent(index, typeof(LeaveTileComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.OnTileElementId = copyComponent.OnTileElementId;
		component.Position = copyComponent.Position;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveLeaveTile()
	{
		RemoveComponent(MessageComponentsLookup.LeaveTile);
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
public sealed partial class MessageMatcher
{
	static JCMG.EntitasRedux.IMatcher<MessageEntity> _matcherLeaveTile;

	public static JCMG.EntitasRedux.IMatcher<MessageEntity> LeaveTile
	{
		get
		{
			if (_matcherLeaveTile == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<MessageEntity>)JCMG.EntitasRedux.Matcher<MessageEntity>.AllOf(MessageComponentsLookup.LeaveTile);
				matcher.ComponentNames = MessageComponentsLookup.ComponentNames;
				_matcherLeaveTile = matcher;
			}

			return _matcherLeaveTile;
		}
	}
}
