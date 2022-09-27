//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity
{
	static readonly TileOwnerComponent TileOwnerComponent = new TileOwnerComponent();

	public bool IsTileOwner
	{
		get { return HasComponent(GameComponentsLookup.TileOwner); }
		set
		{
			if (value != IsTileOwner)
			{
				var index = GameComponentsLookup.TileOwner;
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
public sealed partial class GameMatcher
{
	static JCMG.EntitasRedux.IMatcher<GameEntity> _matcherTileOwner;

	public static JCMG.EntitasRedux.IMatcher<GameEntity> TileOwner
	{
		get
		{
			if (_matcherTileOwner == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameEntity>)JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(GameComponentsLookup.TileOwner);
				matcher.ComponentNames = GameComponentsLookup.ComponentNames;
				_matcherTileOwner = matcher;
			}

			return _matcherTileOwner;
		}
	}
}
