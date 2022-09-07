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
	public TilePositionComponent TilePosition { get { return (TilePositionComponent)GetComponent(TileComponentsLookup.TilePosition); } }
	public bool HasTilePosition { get { return HasComponent(TileComponentsLookup.TilePosition); } }

	public void AddTilePosition(UnityEngine.Vector2Int newValue)
	{
		var index = TileComponentsLookup.TilePosition;
		var component = (TilePositionComponent)CreateComponent(index, typeof(TilePositionComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTilePosition(UnityEngine.Vector2Int newValue)
	{
		var index = TileComponentsLookup.TilePosition;
		var component = (TilePositionComponent)CreateComponent(index, typeof(TilePositionComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTilePositionTo(TilePositionComponent copyComponent)
	{
		var index = TileComponentsLookup.TilePosition;
		var component = (TilePositionComponent)CreateComponent(index, typeof(TilePositionComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = copyComponent.Value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTilePosition()
	{
		RemoveComponent(TileComponentsLookup.TilePosition);
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
	static JCMG.EntitasRedux.IMatcher<TileEntity> _matcherTilePosition;

	public static JCMG.EntitasRedux.IMatcher<TileEntity> TilePosition
	{
		get
		{
			if (_matcherTilePosition == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TileEntity>)JCMG.EntitasRedux.Matcher<TileEntity>.AllOf(TileComponentsLookup.TilePosition);
				matcher.ComponentNames = TileComponentsLookup.ComponentNames;
				_matcherTilePosition = matcher;
			}

			return _matcherTilePosition;
		}
	}
}