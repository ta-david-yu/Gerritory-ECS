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
	public static JCMG.EntitasRedux.IAllOfMatcher<ElementEntity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<ElementEntity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<ElementEntity> AllOf(params JCMG.EntitasRedux.IMatcher<ElementEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<ElementEntity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<ElementEntity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<ElementEntity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<ElementEntity> AnyOf(params JCMG.EntitasRedux.IMatcher<ElementEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<ElementEntity>.AnyOf(matchers);
	}
}
