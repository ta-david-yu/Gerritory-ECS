//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class TileEntity
{
	public OwnerComponent Owner { get { return (OwnerComponent)GetComponent(TileComponentsLookup.Owner); } }
	public bool HasOwner { get { return HasComponent(TileComponentsLookup.Owner); } }

	public void AddOwner(int newOwnerTeamId)
	{
		var index = TileComponentsLookup.Owner;
		var component = (OwnerComponent)CreateComponent(index, typeof(OwnerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.OwnerTeamId = newOwnerTeamId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceOwner(int newOwnerTeamId)
	{
		var index = TileComponentsLookup.Owner;
		var component = (OwnerComponent)CreateComponent(index, typeof(OwnerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.OwnerTeamId = newOwnerTeamId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyOwnerTo(OwnerComponent copyComponent)
	{
		var index = TileComponentsLookup.Owner;
		var component = (OwnerComponent)CreateComponent(index, typeof(OwnerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.OwnerTeamId = copyComponent.OwnerTeamId;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveOwner()
	{
		RemoveComponent(TileComponentsLookup.Owner);
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
public sealed partial class TileMatcher
{
	static JCMG.EntitasRedux.IMatcher<TileEntity> _matcherOwner;

	public static JCMG.EntitasRedux.IMatcher<TileEntity> Owner
	{
		get
		{
			if (_matcherOwner == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TileEntity>)JCMG.EntitasRedux.Matcher<TileEntity>.AllOf(TileComponentsLookup.Owner);
				matcher.ComponentNames = TileComponentsLookup.ComponentNames;
				_matcherOwner = matcher;
			}

			return _matcherOwner;
		}
	}
}
