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
	static readonly EnterableComponent EnterableComponent = new EnterableComponent();

	public bool IsEnterable
	{
		get { return HasComponent(TileComponentsLookup.Enterable); }
		set
		{
			if (value != IsEnterable)
			{
				var index = TileComponentsLookup.Enterable;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: EnterableComponent;

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
public sealed partial class TileMatcher
{
	static JCMG.EntitasRedux.IMatcher<TileEntity> _matcherEnterable;

	public static JCMG.EntitasRedux.IMatcher<TileEntity> Enterable
	{
		get
		{
			if (_matcherEnterable == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TileEntity>)JCMG.EntitasRedux.Matcher<TileEntity>.AllOf(TileComponentsLookup.Enterable);
				matcher.ComponentNames = TileComponentsLookup.ComponentNames;
				_matcherEnterable = matcher;
			}

			return _matcherEnterable;
		}
	}
}