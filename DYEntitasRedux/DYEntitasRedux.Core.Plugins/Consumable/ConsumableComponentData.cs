using Genesis.Plugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYEntitasRedux.Core.Plugins
{
	public class ConsumableComponentData : CodeGeneratorData
	{
		public ConsumableComponentData()
		{
		}

		public ConsumableComponentData(CodeGeneratorData data) : base(data)
		{
		}

		public override string ToString()
		{
			return GetTypeName();
		}

		private const string COMPONENT_TYPE = "ConsumableComponent.Type";

		public string GetTypeName()
		{
			return (string)this[COMPONENT_TYPE];
		}

		public void SetTypeName(string fullTypeName)
		{
			this[COMPONENT_TYPE] = fullTypeName;
		}

		private const string CONSUME_MODE_KEY = "ConsumableComponent.Consume.Data";

		public void SetConsumeMode(ConsumeMode consumeMode)
		{
			this[CONSUME_MODE_KEY] = consumeMode;
		}

		public bool HasConsumeMode()
		{
			return this.ContainsKey(CONSUME_MODE_KEY);
		}

		public void RemoveConsumeMode()
		{
			this.Remove(CONSUME_MODE_KEY);
		}

		
	}
}
