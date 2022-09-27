//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class LevelContext {

	public LevelEntity GameInfoEntity { get { return GetGroup(LevelMatcher.GameInfo).GetSingleEntity(); } }
	public GameInfoComponent GameInfo { get { return GameInfoEntity.GameInfo; } }
	public bool HasGameInfo { get { return GameInfoEntity != null; } }

	public LevelEntity SetGameInfo(int newCurrentHighestTeamScore)
	{
		if (HasGameInfo)
		{
			throw new JCMG.EntitasRedux.EntitasReduxException(
				"Could not set GameInfo!\n" +
				this +
				" already has an entity with GameInfoComponent!",
				"You should check if the context already has a GameInfoEntity before setting it or use context.ReplaceGameInfo().");
		}
		var entity = CreateEntity();
		#if !ENTITAS_REDUX_NO_IMPL
		entity.AddGameInfo(newCurrentHighestTeamScore);
		#endif
		return entity;
	}

	public void ReplaceGameInfo(int newCurrentHighestTeamScore)
	{
		#if !ENTITAS_REDUX_NO_IMPL
		var entity = GameInfoEntity;
		if (entity == null)
		{
			entity = SetGameInfo(newCurrentHighestTeamScore);
		}
		else
		{
			entity.ReplaceGameInfo(newCurrentHighestTeamScore);
		}
		#endif
	}

	public void RemoveGameInfo()
	{
		GameInfoEntity.Destroy();
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
public partial class LevelEntity
{
	public GameInfoComponent GameInfo { get { return (GameInfoComponent)GetComponent(LevelComponentsLookup.GameInfo); } }
	public bool HasGameInfo { get { return HasComponent(LevelComponentsLookup.GameInfo); } }

	public void AddGameInfo(int newCurrentHighestTeamScore)
	{
		var index = LevelComponentsLookup.GameInfo;
		var component = (GameInfoComponent)CreateComponent(index, typeof(GameInfoComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.CurrentHighestTeamScore = newCurrentHighestTeamScore;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceGameInfo(int newCurrentHighestTeamScore)
	{
		var index = LevelComponentsLookup.GameInfo;
		var component = (GameInfoComponent)CreateComponent(index, typeof(GameInfoComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.CurrentHighestTeamScore = newCurrentHighestTeamScore;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyGameInfoTo(GameInfoComponent copyComponent)
	{
		var index = LevelComponentsLookup.GameInfo;
		var component = (GameInfoComponent)CreateComponent(index, typeof(GameInfoComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.CurrentHighestTeamScore = copyComponent.CurrentHighestTeamScore;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveGameInfo()
	{
		RemoveComponent(LevelComponentsLookup.GameInfo);
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
public sealed partial class LevelMatcher
{
	static JCMG.EntitasRedux.IMatcher<LevelEntity> _matcherGameInfo;

	public static JCMG.EntitasRedux.IMatcher<LevelEntity> GameInfo
	{
		get
		{
			if (_matcherGameInfo == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<LevelEntity>)JCMG.EntitasRedux.Matcher<LevelEntity>.AllOf(LevelComponentsLookup.GameInfo);
				matcher.ComponentNames = LevelComponentsLookup.ComponentNames;
				_matcherGameInfo = matcher;
			}

			return _matcherGameInfo;
		}
	}
}
