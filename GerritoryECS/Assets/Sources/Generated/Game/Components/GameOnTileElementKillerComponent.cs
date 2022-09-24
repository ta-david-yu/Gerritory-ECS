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
	static readonly OnTileElementKillerComponent OnTileElementKillerComponent = new OnTileElementKillerComponent();

	public bool IsOnTileElementKiller
	{
		get { return HasComponent(GameComponentsLookup.OnTileElementKiller); }
		set
		{
			if (value != IsOnTileElementKiller)
			{
				var index = GameComponentsLookup.OnTileElementKiller;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: OnTileElementKillerComponent;

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
	static JCMG.EntitasRedux.IMatcher<GameEntity> _matcherOnTileElementKiller;

	public static JCMG.EntitasRedux.IMatcher<GameEntity> OnTileElementKiller
	{
		get
		{
			if (_matcherOnTileElementKiller == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameEntity>)JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(GameComponentsLookup.OnTileElementKiller);
				matcher.ComponentNames = GameComponentsLookup.ComponentNames;
				_matcherOnTileElementKiller = matcher;
			}

			return _matcherOnTileElementKiller;
		}
	}
}