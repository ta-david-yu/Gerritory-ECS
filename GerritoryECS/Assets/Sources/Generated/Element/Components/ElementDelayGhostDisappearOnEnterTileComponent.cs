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
	static readonly DelayGhostDisappearOnEnterTileComponent DelayGhostDisappearOnEnterTileComponent = new DelayGhostDisappearOnEnterTileComponent();

	public bool IsDelayGhostDisappearOnEnterTile
	{
		get { return HasComponent(ElementComponentsLookup.DelayGhostDisappearOnEnterTile); }
		set
		{
			if (value != IsDelayGhostDisappearOnEnterTile)
			{
				var index = ElementComponentsLookup.DelayGhostDisappearOnEnterTile;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: DelayGhostDisappearOnEnterTileComponent;

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
	static JCMG.EntitasRedux.IMatcher<ElementEntity> _matcherDelayGhostDisappearOnEnterTile;

	public static JCMG.EntitasRedux.IMatcher<ElementEntity> DelayGhostDisappearOnEnterTile
	{
		get
		{
			if (_matcherDelayGhostDisappearOnEnterTile == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ElementEntity>)JCMG.EntitasRedux.Matcher<ElementEntity>.AllOf(ElementComponentsLookup.DelayGhostDisappearOnEnterTile);
				matcher.ComponentNames = ElementComponentsLookup.ComponentNames;
				_matcherDelayGhostDisappearOnEnterTile = matcher;
			}

			return _matcherDelayGhostDisappearOnEnterTile;
		}
	}
}
