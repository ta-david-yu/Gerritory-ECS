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

public class EffectCleanupFeature : Feature
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public EffectCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Effect);
	}
	#endif

	public EffectCleanupFeature(IContext<EffectEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<EffectEntity> context)
	{

	}
}
