//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class LevelEventSystems : Feature
{
	public LevelEventSystems(Contexts contexts)
	{
		Add(new LeadingTeamAddedEventSystem(contexts)); // priority: 0
		Add(new AnyTeamGameRankingAddedEventSystem(contexts)); // priority: 0
		Add(new TeamScoreAddedEventSystem(contexts)); // priority: 0
	}
}
