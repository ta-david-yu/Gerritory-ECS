//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameFlowEntity
{
	static readonly PlayingComponent PlayingComponent = new PlayingComponent();

	public bool IsPlaying
	{
		get { return HasComponent(GameFlowComponentsLookup.Playing); }
		set
		{
			if (value != IsPlaying)
			{
				var index = GameFlowComponentsLookup.Playing;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: PlayingComponent;

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
public sealed partial class GameFlowMatcher
{
	static JCMG.EntitasRedux.IMatcher<GameFlowEntity> _matcherPlaying;

	public static JCMG.EntitasRedux.IMatcher<GameFlowEntity> Playing
	{
		get
		{
			if (_matcherPlaying == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameFlowEntity>)JCMG.EntitasRedux.Matcher<GameFlowEntity>.AllOf(GameFlowComponentsLookup.Playing);
				matcher.ComponentNames = GameFlowComponentsLookup.ComponentNames;
				_matcherPlaying = matcher;
			}

			return _matcherPlaying;
		}
	}
}
