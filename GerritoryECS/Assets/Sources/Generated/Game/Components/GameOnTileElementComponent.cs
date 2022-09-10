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
	public OnTileElementComponent OnTileElement { get { return (OnTileElementComponent)GetComponent(GameComponentsLookup.OnTileElement); } }
	public bool HasOnTileElement { get { return HasComponent(GameComponentsLookup.OnTileElement); } }

	public void AddOnTileElement(int newId)
	{
		var index = GameComponentsLookup.OnTileElement;
		var component = (OnTileElementComponent)CreateComponent(index, typeof(OnTileElementComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Id = newId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceOnTileElement(int newId)
	{
		var index = GameComponentsLookup.OnTileElement;
		var component = (OnTileElementComponent)CreateComponent(index, typeof(OnTileElementComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Id = newId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyOnTileElementTo(OnTileElementComponent copyComponent)
	{
		var index = GameComponentsLookup.OnTileElement;
		var component = (OnTileElementComponent)CreateComponent(index, typeof(OnTileElementComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Id = copyComponent.Id;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveOnTileElement()
	{
		RemoveComponent(GameComponentsLookup.OnTileElement);
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
	static JCMG.EntitasRedux.IMatcher<GameEntity> _matcherOnTileElement;

	public static JCMG.EntitasRedux.IMatcher<GameEntity> OnTileElement
	{
		get
		{
			if (_matcherOnTileElement == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameEntity>)JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(GameComponentsLookup.OnTileElement);
				matcher.ComponentNames = GameComponentsLookup.ComponentNames;
				_matcherOnTileElement = matcher;
			}

			return _matcherOnTileElement;
		}
	}
}
