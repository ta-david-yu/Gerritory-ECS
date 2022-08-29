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
	public GameContext Game { get; set; }
	public InputContext Input { get; set; }

	public JCMG.EntitasRedux.IContext[] AllContexts { get { return new JCMG.EntitasRedux.IContext [] { Config, Game, Input }; } }

	public Contexts()
	{
		Config = new ConfigContext();
		Game = new GameContext();
		Input = new InputContext();

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
	public const string MoveOnTile = "MoveOnTile";
	public const string OnTileElement = "OnTileElement";
	public const string Player = "Player";
	public const string TilePosition = "TilePosition";
	public const string UserInput = "UserInput";

	[JCMG.EntitasRedux.PostConstructor]
	public void InitializeEntityIndices()
	{
		Game.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<GameEntity, UnityEngine.Vector2Int>(
			MoveOnTile,
			Game.GetGroup(GameMatcher.MoveOnTile),
			(e, c) => ((MoveOnTileComponent)c).ToPosition));

		Game.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<GameEntity, UnityEngine.Vector2Int>(
			OnTileElement,
			Game.GetGroup(GameMatcher.OnTileElement),
			(e, c) => ((OnTileElementComponent)c).Position));

		Game.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>(
			Player,
			Game.GetGroup(GameMatcher.Player),
			(e, c) => ((PlayerComponent)c).Id));

		Game.AddEntityIndex(new JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, UnityEngine.Vector2Int>(
			TilePosition,
			Game.GetGroup(GameMatcher.TilePosition),
			(e, c) => ((TilePositionComponent)c).Value));

		Input.AddEntityIndex(new JCMG.EntitasRedux.EntityIndex<InputEntity, int>(
			UserInput,
			Input.GetGroup(InputMatcher.UserInput),
			(e, c) => ((UserInputComponent)c).UserIndex));
	}
}

public static class ContextsExtensions
{
	public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithMoveOnTile(this GameContext context, UnityEngine.Vector2Int ToPosition)
	{
		return ((JCMG.EntitasRedux.EntityIndex<GameEntity, UnityEngine.Vector2Int>)context.GetEntityIndex(Contexts.MoveOnTile)).GetEntities(ToPosition);
	}

	public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithOnTileElement(this GameContext context, UnityEngine.Vector2Int Position)
	{
		return ((JCMG.EntitasRedux.EntityIndex<GameEntity, UnityEngine.Vector2Int>)context.GetEntityIndex(Contexts.OnTileElement)).GetEntities(Position);
	}

	public static GameEntity GetEntityWithPlayer(this GameContext context, int Id)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.Player)).GetEntity(Id);
	}

	public static GameEntity GetEntityWithTilePosition(this GameContext context, UnityEngine.Vector2Int Value)
	{
		return ((JCMG.EntitasRedux.PrimaryEntityIndex<GameEntity, UnityEngine.Vector2Int>)context.GetEntityIndex(Contexts.TilePosition)).GetEntity(Value);
	}

	public static System.Collections.Generic.HashSet<InputEntity> GetEntitiesWithUserInput(this InputContext context, int UserIndex)
	{
		return ((JCMG.EntitasRedux.EntityIndex<InputEntity, int>)context.GetEntityIndex(Contexts.UserInput)).GetEntities(UserIndex);
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
			CreateContextObserver(Game);
			CreateContextObserver(Input);
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
