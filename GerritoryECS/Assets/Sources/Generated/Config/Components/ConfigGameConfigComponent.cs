//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ConfigContext {

	public ConfigEntity GameConfigEntity { get { return GetGroup(ConfigMatcher.GameConfig).GetSingleEntity(); } }
	public GameConfigComponent GameConfig { get { return GameConfigEntity.GameConfig; } }
	public bool HasGameConfig { get { return GameConfigEntity != null; } }

	public ConfigEntity SetGameConfig(IGameConfig newValue)
	{
		if (HasGameConfig)
		{
			throw new JCMG.EntitasRedux.EntitasReduxException(
				"Could not set GameConfig!\n" +
				this +
				" already has an entity with GameConfigComponent!",
				"You should check if the context already has a GameConfigEntity before setting it or use context.ReplaceGameConfig().");
		}
		var entity = CreateEntity();
		#if !ENTITAS_REDUX_NO_IMPL
		entity.AddGameConfig(newValue);
		#endif
		return entity;
	}

	public void ReplaceGameConfig(IGameConfig newValue)
	{
		#if !ENTITAS_REDUX_NO_IMPL
		var entity = GameConfigEntity;
		if (entity == null)
		{
			entity = SetGameConfig(newValue);
		}
		else
		{
			entity.ReplaceGameConfig(newValue);
		}
		#endif
	}

	public void RemoveGameConfig()
	{
		GameConfigEntity.Destroy();
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
public partial class ConfigEntity
{
	public GameConfigComponent GameConfig { get { return (GameConfigComponent)GetComponent(ConfigComponentsLookup.GameConfig); } }
	public bool HasGameConfig { get { return HasComponent(ConfigComponentsLookup.GameConfig); } }

	public void AddGameConfig(IGameConfig newValue)
	{
		var index = ConfigComponentsLookup.GameConfig;
		var component = (GameConfigComponent)CreateComponent(index, typeof(GameConfigComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceGameConfig(IGameConfig newValue)
	{
		var index = ConfigComponentsLookup.GameConfig;
		var component = (GameConfigComponent)CreateComponent(index, typeof(GameConfigComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyGameConfigTo(GameConfigComponent copyComponent)
	{
		var index = ConfigComponentsLookup.GameConfig;
		var component = (GameConfigComponent)CreateComponent(index, typeof(GameConfigComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveGameConfig()
	{
		RemoveComponent(ConfigComponentsLookup.GameConfig);
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
public sealed partial class ConfigMatcher
{
	static JCMG.EntitasRedux.IMatcher<ConfigEntity> _matcherGameConfig;

	public static JCMG.EntitasRedux.IMatcher<ConfigEntity> GameConfig
	{
		get
		{
			if (_matcherGameConfig == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ConfigEntity>)JCMG.EntitasRedux.Matcher<ConfigEntity>.AllOf(ConfigComponentsLookup.GameConfig);
				matcher.ComponentNames = ConfigComponentsLookup.ComponentNames;
				_matcherGameConfig = matcher;
			}

			return _matcherGameConfig;
		}
	}
}
