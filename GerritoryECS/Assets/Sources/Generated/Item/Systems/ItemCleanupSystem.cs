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

public class ItemCleanupSystems : JCMG.EntitasRedux.Systems
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public ItemCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Item;
		_cleanupSystems.Add(new RemoveEatenFromItemEntitiesSystem(context));
	}
	#endif

	public ItemCleanupSystems(IContext<ItemEntity> context) : base()
	{
		_cleanupSystems.Add(new RemoveEatenFromItemEntitiesSystem(context));
	}
}