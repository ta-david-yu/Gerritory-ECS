//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class MessageEntity
{
	static readonly LeaveBecauseOfDeathComponent LeaveBecauseOfDeathComponent = new LeaveBecauseOfDeathComponent();

	public bool IsLeaveBecauseOfDeath
	{
		get { return HasComponent(MessageComponentsLookup.LeaveBecauseOfDeath); }
		set
		{
			if (value != IsLeaveBecauseOfDeath)
			{
				var index = MessageComponentsLookup.LeaveBecauseOfDeath;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: LeaveBecauseOfDeathComponent;

					AddComponent(index, component);
				}
				else
				{
					RemoveComponent(index);
				}
			}
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
public sealed partial class MessageMatcher
{
	static JCMG.EntitasRedux.IMatcher<MessageEntity> _matcherLeaveBecauseOfDeath;

	public static JCMG.EntitasRedux.IMatcher<MessageEntity> LeaveBecauseOfDeath
	{
		get
		{
			if (_matcherLeaveBecauseOfDeath == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<MessageEntity>)JCMG.EntitasRedux.Matcher<MessageEntity>.AllOf(MessageComponentsLookup.LeaveBecauseOfDeath);
				matcher.ComponentNames = MessageComponentsLookup.ComponentNames;
				_matcherLeaveBecauseOfDeath = matcher;
			}

			return _matcherLeaveBecauseOfDeath;
		}
	}
}
