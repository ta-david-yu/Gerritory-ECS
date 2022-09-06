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
	public OnTileItemComponent OnTileItem { get { return (OnTileItemComponent)GetComponent(ItemComponentsLookup.OnTileItem); } }
	public bool HasOnTileItem { get { return HasComponent(ItemComponentsLookup.OnTileItem); } }

	public void AddOnTileItem(UnityEngine.Vector2Int newPosition)
	{
		var index = ItemComponentsLookup.OnTileItem;
		var component = (OnTileItemComponent)CreateComponent(index, typeof(OnTileItemComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Position = newPosition;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceOnTileItem(UnityEngine.Vector2Int newPosition)
	{
		var index = ItemComponentsLookup.OnTileItem;
		var component = (OnTileItemComponent)CreateComponent(index, typeof(OnTileItemComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Position = newPosition;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyOnTileItemTo(OnTileItemComponent copyComponent)
	{
		var index = ItemComponentsLookup.OnTileItem;
		var component = (OnTileItemComponent)CreateComponent(index, typeof(OnTileItemComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Position = copyComponent.Position;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveOnTileItem()
	{
		RemoveComponent(ItemComponentsLookup.OnTileItem);
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
	static JCMG.EntitasRedux.IMatcher<ItemEntity> _matcherOnTileItem;

	public static JCMG.EntitasRedux.IMatcher<ItemEntity> OnTileItem
	{
		get
		{
			if (_matcherOnTileItem == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ItemEntity>)JCMG.EntitasRedux.Matcher<ItemEntity>.AllOf(ItemComponentsLookup.OnTileItem);
				matcher.ComponentNames = ItemComponentsLookup.ComponentNames;
				_matcherOnTileItem = matcher;
			}

			return _matcherOnTileItem;
		}
	}
}
