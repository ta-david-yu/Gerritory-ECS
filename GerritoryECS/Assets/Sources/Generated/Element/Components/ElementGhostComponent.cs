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
	static readonly GhostComponent GhostComponent = new GhostComponent();

	public bool IsGhost
	{
		get { return HasComponent(ElementComponentsLookup.Ghost); }
		set
		{
			if (value != IsGhost)
			{
				var index = ElementComponentsLookup.Ghost;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: GhostComponent;

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
	static JCMG.EntitasRedux.IMatcher<ElementEntity> _matcherGhost;

	public static JCMG.EntitasRedux.IMatcher<ElementEntity> Ghost
	{
		get
		{
			if (_matcherGhost == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ElementEntity>)JCMG.EntitasRedux.Matcher<ElementEntity>.AllOf(ElementComponentsLookup.Ghost);
				matcher.ComponentNames = ElementComponentsLookup.ComponentNames;
				_matcherGhost = matcher;
			}

			return _matcherGhost;
		}
	}
}
