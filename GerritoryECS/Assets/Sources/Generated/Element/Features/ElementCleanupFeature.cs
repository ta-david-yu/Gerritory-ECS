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

public class ElementCleanupFeature : Feature
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public ElementCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Element);
	}
	#endif

	public ElementCleanupFeature(IContext<ElementEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<ElementEntity> context)
	{
		Add(new RemoveLeaveStateFromElementEntitiesSystem(context));
		Add(new RemoveMoveOnTileEndFromElementEntitiesSystem(context));
		Add(new RemoveEnterStateFromElementEntitiesSystem(context));
		Add(new RemoveMoveOnTileBeginFromElementEntitiesSystem(context));
	}
}
