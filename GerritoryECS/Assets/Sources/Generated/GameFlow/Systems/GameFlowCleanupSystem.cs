//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using JCMG.EntitasRedux;

public class GameFlowCleanupSystems : JCMG.EntitasRedux.Systems
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public GameFlowCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.GameFlow;

	}
	#endif

	public GameFlowCleanupSystems(IContext<GameFlowEntity> context) : base()
	{

	}
}
