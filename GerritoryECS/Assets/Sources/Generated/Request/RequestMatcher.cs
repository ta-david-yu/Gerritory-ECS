//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class RequestMatcher
{
	public static JCMG.EntitasRedux.IAllOfMatcher<RequestEntity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<RequestEntity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<RequestEntity> AllOf(params JCMG.EntitasRedux.IMatcher<RequestEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<RequestEntity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<RequestEntity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<RequestEntity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<RequestEntity> AnyOf(params JCMG.EntitasRedux.IMatcher<RequestEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<RequestEntity>.AnyOf(matchers);
	}
}
