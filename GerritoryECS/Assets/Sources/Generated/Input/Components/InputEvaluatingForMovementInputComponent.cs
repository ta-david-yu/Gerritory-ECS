//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InputEntity
{
	static readonly EvaluatingForMovementInputComponent EvaluatingForMovementInputComponent = new EvaluatingForMovementInputComponent();

	public bool IsEvaluatingForMovementInput
	{
		get { return HasComponent(InputComponentsLookup.EvaluatingForMovementInput); }
		set
		{
			if (value != IsEvaluatingForMovementInput)
			{
				var index = InputComponentsLookup.EvaluatingForMovementInput;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: EvaluatingForMovementInputComponent;

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
public sealed partial class InputMatcher
{
	static JCMG.EntitasRedux.IMatcher<InputEntity> _matcherEvaluatingForMovementInput;

	public static JCMG.EntitasRedux.IMatcher<InputEntity> EvaluatingForMovementInput
	{
		get
		{
			if (_matcherEvaluatingForMovementInput == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<InputEntity>)JCMG.EntitasRedux.Matcher<InputEntity>.AllOf(InputComponentsLookup.EvaluatingForMovementInput);
				matcher.ComponentNames = InputComponentsLookup.ComponentNames;
				_matcherEvaluatingForMovementInput = matcher;
			}

			return _matcherEvaluatingForMovementInput;
		}
	}
}