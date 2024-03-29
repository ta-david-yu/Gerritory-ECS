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
	public LeadingTeamComponent LeadingTeam { get { return (LeadingTeamComponent)GetComponent(LevelComponentsLookup.LeadingTeam); } }
	public bool HasLeadingTeam { get { return HasComponent(LevelComponentsLookup.LeadingTeam); } }

	public void AddLeadingTeam(int newTeamId)
	{
		var index = LevelComponentsLookup.LeadingTeam;
		var component = (LeadingTeamComponent)CreateComponent(index, typeof(LeadingTeamComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.TeamId = newTeamId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceLeadingTeam(int newTeamId)
	{
		var index = LevelComponentsLookup.LeadingTeam;
		var component = (LeadingTeamComponent)CreateComponent(index, typeof(LeadingTeamComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.TeamId = newTeamId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyLeadingTeamTo(LeadingTeamComponent copyComponent)
	{
		var index = LevelComponentsLookup.LeadingTeam;
		var component = (LeadingTeamComponent)CreateComponent(index, typeof(LeadingTeamComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.TeamId = copyComponent.TeamId;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveLeadingTeam()
	{
		RemoveComponent(LevelComponentsLookup.LeadingTeam);
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
	static JCMG.EntitasRedux.IMatcher<LevelEntity> _matcherLeadingTeam;

	public static JCMG.EntitasRedux.IMatcher<LevelEntity> LeadingTeam
	{
		get
		{
			if (_matcherLeadingTeam == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<LevelEntity>)JCMG.EntitasRedux.Matcher<LevelEntity>.AllOf(LevelComponentsLookup.LeadingTeam);
				matcher.ComponentNames = LevelComponentsLookup.ComponentNames;
				_matcherLeadingTeam = matcher;
			}

			return _matcherLeadingTeam;
		}
	}
}
