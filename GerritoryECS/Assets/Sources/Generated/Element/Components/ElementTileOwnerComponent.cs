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
	static readonly TileOwnerComponent TileOwnerComponent = new TileOwnerComponent();

	public bool IsTileOwner
	{
		get { return HasComponent(ElementComponentsLookup.TileOwner); }
		set
		{
			if (value != IsTileOwner)
			{
				var index = ElementComponentsLookup.TileOwner;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: TileOwnerComponent;

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
	static JCMG.EntitasRedux.IMatcher<ElementEntity> _matcherTileOwner;

	public static JCMG.EntitasRedux.IMatcher<ElementEntity> TileOwner
	{
		get
		{
			if (_matcherTileOwner == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ElementEntity>)JCMG.EntitasRedux.Matcher<ElementEntity>.AllOf(ElementComponentsLookup.TileOwner);
				matcher.ComponentNames = ElementComponentsLookup.ComponentNames;
				_matcherTileOwner = matcher;
			}

			return _matcherTileOwner;
		}
	}
}
