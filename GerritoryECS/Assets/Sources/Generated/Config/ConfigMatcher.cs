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
	public static JCMG.EntitasRedux.IAllOfMatcher<ConfigEntity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<ConfigEntity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<ConfigEntity> AllOf(params JCMG.EntitasRedux.IMatcher<ConfigEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<ConfigEntity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<ConfigEntity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<ConfigEntity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<ConfigEntity> AnyOf(params JCMG.EntitasRedux.IMatcher<ConfigEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<ConfigEntity>.AnyOf(matchers);
	}
}