using System;
using System.Collections.Generic;
using System.Text;

namespace DYEntitasRedux
{
	[AttributeUsage(
		AttributeTargets.Interface |
		AttributeTargets.Class |
		AttributeTargets.Struct |
		AttributeTargets.Enum,
		Inherited = true)]
	public class ConsumeAttribute : Attribute
	{
		public ConsumeMode ConsumeMode { get; }

		public ConsumeAttribute(ConsumeMode consumeMode)
		{
			ConsumeMode = consumeMode;
		}
	}
}
