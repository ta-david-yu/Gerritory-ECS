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
	public TeamInfoComponent TeamInfo { get { return (TeamInfoComponent)GetComponent(LevelComponentsLookup.TeamInfo); } }
	public bool HasTeamInfo { get { return HasComponent(LevelComponentsLookup.TeamInfo); } }

	public void AddTeamInfo(int newId)
	{
		var index = LevelComponentsLookup.TeamInfo;
		var component = (TeamInfoComponent)CreateComponent(index, typeof(TeamInfoComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Id = newId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTeamInfo(int newId)
	{
		var index = LevelComponentsLookup.TeamInfo;
		var component = (TeamInfoComponent)CreateComponent(index, typeof(TeamInfoComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Id = newId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTeamInfoTo(TeamInfoComponent copyComponent)
	{
		var index = LevelComponentsLookup.TeamInfo;
		var component = (TeamInfoComponent)CreateComponent(index, typeof(TeamInfoComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Id = copyComponent.Id;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTeamInfo()
	{
		RemoveComponent(LevelComponentsLookup.TeamInfo);
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
	static JCMG.EntitasRedux.IMatcher<LevelEntity> _matcherTeamInfo;

	public static JCMG.EntitasRedux.IMatcher<LevelEntity> TeamInfo
	{
		get
		{
			if (_matcherTeamInfo == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<LevelEntity>)JCMG.EntitasRedux.Matcher<LevelEntity>.AllOf(LevelComponentsLookup.TeamInfo);
				matcher.ComponentNames = LevelComponentsLookup.ComponentNames;
				_matcherTeamInfo = matcher;
			}

			return _matcherTeamInfo;
		}
	}
}
