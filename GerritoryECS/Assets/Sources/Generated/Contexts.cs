//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts : JCMG.EntitasRedux.IContexts
{
	#if UNITY_EDITOR && !ENTITAS_REDUX_NO_SHARED_CONTEXT

	static Contexts()
	{
		UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
	}

	/// <summary>
	/// Invoked when the Unity Editor has a <see cref="UnityEditor.PlayModeStateChange"/> change.
	/// </summary>
	private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange playModeStateChange)
	{
		// When entering edit-mode, reset all static state so that it does not interfere with the
		// next play-mode session.
		if (playModeStateChange == UnityEditor.PlayModeStateChange.EnteredEditMode)
		{
			_sharedInstance = null;
		}
	}

	#endif

	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	/// <summary>
	/// A globally-accessible singleton instance of <see cref="Contexts"/>. Instantiated
	/// the first time its <see langword="get"/> property is used.
	/// </summary>
	/// <remarks>
	/// If your project forbids global singletons like this one, add a <c>#define</c> named <c>ENTITAS_REDUX_NO_SHARED_CONTEXT</c>
	/// to its build settings. Doing so will remove this property to prevent accidental use.
	/// </remarks>
	public static Contexts SharedInstance
	{
		get
		{
			if (_sharedInstance == null)
			{
				_sharedInstance = new Contexts();
			}

			return _sharedInstance;
		}
		set	{ _sharedInstance = value; }
	}

	static Contexts _sharedInstance;
	#endif

	public ConfigContext Config { get; set; }
	public EffectContext Effect { get; set; }
	public GameContext Game { get; set; }
	public InputContext Input { get; set; }
	public ItemContext Item { get; set; }
	public LevelContext Level { get; set; }
	public MessageContext Message { get; set; }
	public PlayerStateContext PlayerState { get; set; }
	public TileContext Tile { get; set; }

	public JCMG.EntitasRedux.IContext[] AllContexts { get { return new JCMG.EntitasRedux.IContext [] { Config, Effect, Game, Input, Item, Level, Message, PlayerState, Tile }; } }

	public Contexts()
	{
		Config = new ConfigContext();
		Effect = new EffectContext();
		Game = new GameContext();
		Input = new InputContext();
		Item = new ItemContext();
		Level = new LevelContext();
		Message = new MessageContext();
		PlayerState = new PlayerStateContext();
		Tile = new TileContext();

		var postConstructors = System.Linq.Enumerable.Where(
			GetType().GetMethods(),
			method => System.Attribute.IsDefined(method, typeof(JCMG.EntitasRedux.PostConstructorAttribute))
		);

		foreach (var postConstructor in postConstructors)
		{
			postConstructor.Invoke(this, null);
		}
	}

	public void Reset()
	{
		var contexts = AllContexts;
		for (int i = 0; i < contexts.Length; i++)
		{
			contexts[i].Reset();
		}
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
public partial class Contexts
{
	public const string CanBeRespawnedOn = "CanBeRespawnedOn";
	public const string Eaten = "Eaten";
	public const string ItemEater = "ItemEater";
	public const string MoveOnTile = "MoveOnTile";
	public const string OnTileElement = "OnTileElement";
	public const string OnTileElementEffect = "OnTileElementEffect";
	public const string OnTileElementEnterTile = "OnTileElementEnterTile";
	public const string OnTileElementLeaveTile = "OnTileElementLeaveTile";
	public const string OnTileItem = "OnTileItem";
	public const string OnTilePosition = "OnTilePosition";
	public const string Owner = "Owner";
	public const string Player = "Player";
	public const string State = "State";
	public const string StateHolder = "StateHolder";
	public const string Team = "Team";
	public const string TeamInfo = "TeamInfo";
	public const string TilePosition = "TilePosition";
	public const string UserInput = "UserInput";

	[JCMG.EntitasRedux.PostConstructor]
	public void InitializeEntityIndices()
	{
		Tile.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<TileEntity, int>(
			CanBeRespawnedOn,
			Tile.GetGroup(TileMatcher.CanBeRespawnedOn),
			(e, c) => ((CanBeRespawnedOnComponent)c).RespawnAreaId));

		Item.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<ItemEntity, int>(
			Eaten,
			Item.GetGroup(ItemMatcher.Eaten),
			(e, c) => ((EatenComponent)c).EaterId));

		Game.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>(
			ItemEater,
			Game.GetGroup(GameMatcher.ItemEater),
			(e, c) => ((ItemEaterComponent)c).Id));

		Game.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<GameEntity, UnityEngine.Vector2Int>(
			MoveOnTile,
			Game.GetGroup(GameMatcher.MoveOnTile),
			(e, c) => ((MoveOnTileComponent)c).ToPosition));

		Game.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>(
			OnTileElement,
			Game.GetGroup(GameMatcher.OnTileElement),
			(e, c) => ((OnTileElementComponent)c).Id));

		Effect.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<EffectEntity, int>(
			OnTileElementEffect,
			Effect.GetGroup(EffectMatcher.OnTileElementEffect),
			(e, c) => ((OnTileElementEffectComponent)c).OnTileElementId));

		Message.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<MessageEntity, int>(
			OnTileElementEnterTile,
			Message.GetGroup(MessageMatcher.OnTileElementEnterTile),
			(e, c) => ((OnTileElementEnterTileComponent)c).OnTileElementId));

		Message.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<MessageEntity, int>(
			OnTileElementLeaveTile,
			Message.GetGroup(MessageMatcher.OnTileElementLeaveTile),
			(e, c) => ((OnTileElementLeaveTileComponent)c).OnTileElementId));

		Item.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<ItemEntity, UnityEngine.Vector2Int>(
			OnTileItem,
			Item.GetGroup(ItemMatcher.OnTileItem),
			(e, c) => ((OnTileItemComponent)c).Position));

		Game.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<GameEntity, UnityEngine.Vector2Int>(
			OnTilePosition,
			Game.GetGroup(GameMatcher.OnTilePosition),
			(e, c) => ((OnTilePositionComponent)c).Value));

		Tile.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<TileEntity, int>(
			Owner,
			Tile.GetGroup(TileMatcher.Owner),
			(e, c) => ((OwnerComponent)c).OwnerTeamId));

		Game.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>(
			Player,
			Game.GetGroup(GameMatcher.Player),
			(e, c) => ((PlayerComponent)c).Id));

		PlayerState.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<PlayerStateEntity, int>(
			State,
			PlayerState.GetGroup(PlayerStateMatcher.State),
			(e, c) => ((StateComponent)c).HolderId));

		Game.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>(
			StateHolder,
			Game.GetGroup(GameMatcher.StateHolder),
			(e, c) => ((StateHolderComponent)c).Id));

		Game.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<GameEntity, int>(
			Team,
			Game.GetGroup(GameMatcher.Team),
			(e, c) => ((TeamComponent)c).Id));

		Level.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<LevelEntity, int>(
			TeamInfo,
			Level.GetGroup(LevelMatcher.TeamInfo),
			(e, c) => ((TeamInfoComponent)c).Id));

		Tile.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<TileEntity, UnityEngine.Vector2Int>(
			TilePosition,
			Tile.GetGroup(TileMatcher.TilePosition),
			(e, c) => ((TilePositionComponent)c).Value));

		Input.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<InputEntity, int>(
			UserInput,
			Input.GetGroup(InputMatcher.UserInput),
			(e, c) => ((UserInputComponent)c).UserId));
	}
}

public static class ContextsExtensions
{
	public static System.Collections.Generic.HashSet<TileEntity> GetEntitiesWithCanBeRespawnedOn(this TileContext context, int RespawnAreaId)
	{
		return ((JCMG.EntitasRedux.EntityIndex<TileEntity, int>)context.GetEntityIndex(Contexts.CanBeRespawnedOn)).GetEntities(RespawnAreaId);
	}

	public static System.Collections.Generic.HashSet<ItemEntity> GetEntitiesWithEaten(this ItemContext context, int EaterId)
	{
		return ((JCMG.EntitasRedux.EntityIndex<ItemEntity, int>)context.GetEntityIndex(Contexts.Eaten)).GetEntities(EaterId);
	}

	public static GameEntity GetEntityWithItemEater(this GameContext context, int Id)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.ItemEater)).GetEntity(Id);
	}

	public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithMoveOnTile(this GameContext context, UnityEngine.Vector2Int ToPosition)
	{
		return ((JCMG.EntitasRedux.EntityIndex<GameEntity, UnityEngine.Vector2Int>)context.GetEntityIndex(Contexts.MoveOnTile)).GetEntities(ToPosition);
	}

	public static GameEntity GetEntityWithOnTileElement(this GameContext context, int Id)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.OnTileElement)).GetEntity(Id);
	}

	public static System.Collections.Generic.HashSet<EffectEntity> GetEntitiesWithOnTileElementEffect(this EffectContext context, int OnTileElementId)
	{
		return ((JCMG.EntitasRedux.EntityIndex<EffectEntity, int>)context.GetEntityIndex(Contexts.OnTileElementEffect)).GetEntities(OnTileElementId);
	}

	public static MessageEntity GetEntityWithOnTileElementEnterTile(this MessageContext context, int OnTileElementId)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<MessageEntity, int>)context.GetEntityIndex(Contexts.OnTileElementEnterTile)).GetEntity(OnTileElementId);
	}

	public static MessageEntity GetEntityWithOnTileElementLeaveTile(this MessageContext context, int OnTileElementId)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<MessageEntity, int>)context.GetEntityIndex(Contexts.OnTileElementLeaveTile)).GetEntity(OnTileElementId);
	}

	public static ItemEntity GetEntityWithOnTileItem(this ItemContext context, UnityEngine.Vector2Int Position)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<ItemEntity, UnityEngine.Vector2Int>)context.GetEntityIndex(Contexts.OnTileItem)).GetEntity(Position);
	}

	public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithOnTilePosition(this GameContext context, UnityEngine.Vector2Int Value)
	{
		return ((JCMG.EntitasRedux.EntityIndex<GameEntity, UnityEngine.Vector2Int>)context.GetEntityIndex(Contexts.OnTilePosition)).GetEntities(Value);
	}

	public static System.Collections.Generic.HashSet<TileEntity> GetEntitiesWithOwner(this TileContext context, int OwnerTeamId)
	{
		return ((JCMG.EntitasRedux.EntityIndex<TileEntity, int>)context.GetEntityIndex(Contexts.Owner)).GetEntities(OwnerTeamId);
	}

	public static GameEntity GetEntityWithPlayer(this GameContext context, int Id)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.Player)).GetEntity(Id);
	}

	public static System.Collections.Generic.HashSet<PlayerStateEntity> GetEntitiesWithState(this PlayerStateContext context, int HolderId)
	{
		return ((JCMG.EntitasRedux.EntityIndex<PlayerStateEntity, int>)context.GetEntityIndex(Contexts.State)).GetEntities(HolderId);
	}

	public static GameEntity GetEntityWithStateHolder(this GameContext context, int Id)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.StateHolder)).GetEntity(Id);
	}

	public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithTeam(this GameContext context, int Id)
	{
		return ((JCMG.EntitasRedux.EntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.Team)).GetEntities(Id);
	}

	public static LevelEntity GetEntityWithTeamInfo(this LevelContext context, int Id)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<LevelEntity, int>)context.GetEntityIndex(Contexts.TeamInfo)).GetEntity(Id);
	}

	public static TileEntity GetEntityWithTilePosition(this TileContext context, UnityEngine.Vector2Int Value)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<TileEntity, UnityEngine.Vector2Int>)context.GetEntityIndex(Contexts.TilePosition)).GetEntity(Value);
	}

	public static System.Collections.Generic.HashSet<InputEntity> GetEntitiesWithUserInput(this InputContext context, int UserId)
	{
		return ((JCMG.EntitasRedux.EntityIndex<InputEntity, int>)context.GetEntityIndex(Contexts.UserInput)).GetEntities(UserId);
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
public partial class Contexts {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

	[JCMG.EntitasRedux.PostConstructor]
	public void InitializeContextObservers() {
		try {
			CreateContextObserver(Config);
			CreateContextObserver(Effect);
			CreateContextObserver(Game);
			CreateContextObserver(Input);
			CreateContextObserver(Item);
			CreateContextObserver(Level);
			CreateContextObserver(Message);
			CreateContextObserver(PlayerState);
			CreateContextObserver(Tile);
		} catch(System.Exception) {
		}
	}

	public void CreateContextObserver(JCMG.EntitasRedux.IContext context) {
		if (UnityEngine.Application.isPlaying) {
			var observer = new JCMG.EntitasRedux.VisualDebugging.ContextObserver(context);
			UnityEngine.Object.DontDestroyOnLoad(observer.GameObject);
		}
	}

#endif
}
