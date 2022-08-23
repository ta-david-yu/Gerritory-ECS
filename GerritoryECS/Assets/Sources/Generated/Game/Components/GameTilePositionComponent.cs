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
	public TilePositionComponent TilePosition { get { return (TilePositionComponent)GetComponent(GameComponentsLookup.TilePosition); } }
	public bool HasTilePosition { get { return HasComponent(GameComponentsLookup.TilePosition); } }

	public void AddTilePosition(UnityEngine.Vector2Int newValue)
	{
		var index = GameComponentsLookup.TilePosition;
		var component = (TilePositionComponent)CreateComponent(index, typeof(TilePositionComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTilePosition(UnityEngine.Vector2Int newValue)
	{
		var index = GameComponentsLookup.TilePosition;
		var component = (TilePositionComponent)CreateComponent(index, typeof(TilePositionComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTilePositionTo(TilePositionComponent copyComponent)
	{
		var index = GameComponentsLookup.TilePosition;
		var component = (TilePositionComponent)CreateComponent(index, typeof(TilePositionComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = copyComponent.Value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTilePosition()
	{
		RemoveComponent(GameComponentsLookup.TilePosition);
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
	static JCMG.EntitasRedux.IMatcher<GameEntity> _matcherTilePosition;

	public static JCMG.EntitasRedux.IMatcher<GameEntity> TilePosition
	{
		get
		{
			if (_matcherTilePosition == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameEntity>)JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(GameComponentsLookup.TilePosition);
				matcher.ComponentNames = GameComponentsLookup.ComponentNames;
				_matcherTilePosition = matcher;
			}

			return _matcherTilePosition;
		}
	}
}
