//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class ElementEventSystems : Feature
{
	public ElementEventSystems(Contexts contexts)
	{
		Add(new DeadAddedEventSystem(contexts)); // priority: 0
		Add(new DeadRemovedEventSystem(contexts)); // priority: 0
		Add(new MoveOnTileAddedEventSystem(contexts)); // priority: 0
		Add(new MoveOnTileBeginAddedEventSystem(contexts)); // priority: 0
		Add(new MoveOnTileEndAddedEventSystem(contexts)); // priority: 0
		Add(new OnTilePositionAddedEventSystem(contexts)); // priority: 0
		Add(new TeamAddedEventSystem(contexts)); // priority: 0
	}
}