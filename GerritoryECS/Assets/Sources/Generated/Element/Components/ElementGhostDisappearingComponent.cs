//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ElementEntity
{
	public GhostDisappearingComponent GhostDisappearing { get { return (GhostDisappearingComponent)GetComponent(ElementComponentsLookup.GhostDisappearing); } }
	public bool HasGhostDisappearing { get { return HasComponent(ElementComponentsLookup.GhostDisappearing); } }

	public void AddGhostDisappearing(float newProgress)
	{
		var index = ElementComponentsLookup.GhostDisappearing;
		var component = (GhostDisappearingComponent)CreateComponent(index, typeof(GhostDisappearingComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Progress = newProgress;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceGhostDisappearing(float newProgress)
	{
		var index = ElementComponentsLookup.GhostDisappearing;
		var component = (GhostDisappearingComponent)CreateComponent(index, typeof(GhostDisappearingComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Progress = newProgress;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyGhostDisappearingTo(GhostDisappearingComponent copyComponent)
	{
		var index = ElementComponentsLookup.GhostDisappearing;
		var component = (GhostDisappearingComponent)CreateComponent(index, typeof(GhostDisappearingComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Progress = copyComponent.Progress;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveGhostDisappearing()
	{
		RemoveComponent(ElementComponentsLookup.GhostDisappearing);
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
public sealed partial class ElementMatcher
{
	static JCMG.EntitasRedux.IMatcher<ElementEntity> _matcherGhostDisappearing;

	public static JCMG.EntitasRedux.IMatcher<ElementEntity> GhostDisappearing
	{
		get
		{
			if (_matcherGhostDisappearing == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ElementEntity>)JCMG.EntitasRedux.Matcher<ElementEntity>.AllOf(ElementComponentsLookup.GhostDisappearing);
				matcher.ComponentNames = ElementComponentsLookup.ComponentNames;
				_matcherGhostDisappearing = matcher;
			}

			return _matcherGhostDisappearing;
		}
	}
}
