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
	public ConstructPlayerComponent ConstructPlayer { get { return (ConstructPlayerComponent)GetComponent(LevelComponentsLookup.ConstructPlayer); } }
	public bool HasConstructPlayer { get { return HasComponent(LevelComponentsLookup.ConstructPlayer); } }

	public void AddConstructPlayer(int newPlayerId, string newPlayerName, int newTeamId, int newSkinId, bool newIsAI)
	{
		var index = LevelComponentsLookup.ConstructPlayer;
		var component = (ConstructPlayerComponent)CreateComponent(index, typeof(ConstructPlayerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.PlayerId = newPlayerId;
		component.PlayerName = newPlayerName;
		component.TeamId = newTeamId;
		component.SkinId = newSkinId;
		component.IsAI = newIsAI;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceConstructPlayer(int newPlayerId, string newPlayerName, int newTeamId, int newSkinId, bool newIsAI)
	{
		var index = LevelComponentsLookup.ConstructPlayer;
		var component = (ConstructPlayerComponent)CreateComponent(index, typeof(ConstructPlayerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.PlayerId = newPlayerId;
		component.PlayerName = newPlayerName;
		component.TeamId = newTeamId;
		component.SkinId = newSkinId;
		component.IsAI = newIsAI;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyConstructPlayerTo(ConstructPlayerComponent copyComponent)
	{
		var index = LevelComponentsLookup.ConstructPlayer;
		var component = (ConstructPlayerComponent)CreateComponent(index, typeof(ConstructPlayerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.PlayerId = copyComponent.PlayerId;
		component.PlayerName = copyComponent.PlayerName;
		component.TeamId = copyComponent.TeamId;
		component.SkinId = copyComponent.SkinId;
		component.IsAI = copyComponent.IsAI;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveConstructPlayer()
	{
		RemoveComponent(LevelComponentsLookup.ConstructPlayer);
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
	static JCMG.EntitasRedux.IMatcher<LevelEntity> _matcherConstructPlayer;

	public static JCMG.EntitasRedux.IMatcher<LevelEntity> ConstructPlayer
	{
		get
		{
			if (_matcherConstructPlayer == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<LevelEntity>)JCMG.EntitasRedux.Matcher<LevelEntity>.AllOf(LevelComponentsLookup.ConstructPlayer);
				matcher.ComponentNames = LevelComponentsLookup.ComponentNames;
				_matcherConstructPlayer = matcher;
			}

			return _matcherConstructPlayer;
		}
	}
}
