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

public class ItemCleanupFeature : Feature
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public ItemCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Item);
	}
	#endif

	public ItemCleanupFeature(IContext<ItemEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<ItemEntity> context)
	{

	}
}
