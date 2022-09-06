using System.Collections.Generic;
using System.IO;
using System.Linq;
using EntitasRedux.Core.Plugins;
using Genesis.Plugin;

namespace EntitasReduxCodeGeneratorPlugin.Item.CodeGenerators
{
	/*
	internal sealed class PowerupLookupGenerator : AbstractGenerator
	{
		public override string Name => "PowerupLookupGenerator";

		public override CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return new CodeGenFile[0];
		}
	}*/
}

/*
namespace EntitasRedux.Core.Plugins
{
	internal sealed class PowerupLookupGenerator : AbstractGenerator
	{
		public override string Name
		{
			get { return "Powerup (Lookup)"; }
		}

		private const string k_LookUpTableClassName = "PowerupsLookup";

		private const string TEMPLATE =
@"using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public static class ${Lookup}
{
${componentConstantsList}

${totalComponentsConstant}

	public static readonly string[] ComponentNames =
	{
${componentNamesList}
	};

	public static readonly System.Type[] ComponentTypes =
	{
${componentTypesList}
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
${componentTypeToIndexLookup}
	};

	/// <summary>
	/// Returns a component index based on the passed <paramref name=""component""/> type; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name=""component""></param>
	public static int GetComponentIndex(IComponent component)
	{
		return GetComponentIndex(component.GetType());
	}

	/// <summary>
	/// Returns a component index based on the passed <paramref name=""componentType""/>; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name=""componentType""></param>
	public static int GetComponentIndex(Type componentType)
	{
		return ComponentTypeToIndex.TryGetValue(componentType, out var index) ? index : -1;
	}
}
";

		private const string POWERUP_COMPONENT_CONSTANT_TEMPLATE = @"	public const int ${PowerupComponentName} = ${Index};";
		private const string TOTAL_POWERUPS_CONSTANT_TEMPLATE = @"	public const int NumberOfPowerups = ${numberOfPowerups};";
		private const string POWERUP_NAME_TEMPLATE = @"		""${ComponentName}""";
		private const string COMPONENT_TYPE_TEMPLATE = @"		typeof(${ComponentType})";
		private const string COMPONENT_TYPE_TO_INDEX_TEMPLATE = "		{ typeof(${ComponentType}), ${Index} }";

		public override CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var codeGenFilesResult = new List<CodeGenFile>();

			var lookup = GeneratePowerupsLookupClassFromPowerupComponent(
				data
					.OfType<ComponentData>()
					.Where(d => d.ShouldGenerateIndex() && d.GetContextNames().Contains("Item"))
					.ToArray());

			codeGenFilesResult.Add(lookup);

			return codeGenFilesResult.ToArray();
		}


		private CodeGenFile GeneratePowerupsLookupClassFromPowerupComponent(ComponentData[] data)
		{
			var componentConstantsList = string.Join(
				"\n",
				data
					.Select(
						(d, index) => POWERUP_COMPONENT_CONSTANT_TEMPLATE
							.Replace("${PowerupComponentName}", d.ComponentName())
							.Replace("${Index}", index.ToString()))
					.ToArray());

			var totalComponentsConstant = TOTAL_POWERUPS_CONSTANT_TEMPLATE
				.Replace("${numberOfPowerups}", data.Length.ToString());

			var componentNamesList = string.Join(
				",\n",
				data
					.Select(
						d => POWERUP_NAME_TEMPLATE
							.Replace("${ComponentName}", d.ComponentName()))
					.ToArray());

			var componentTypesList = string.Join(
				",\n",
				data
					.Select(
						d => COMPONENT_TYPE_TEMPLATE
							.Replace("${ComponentType}", d.GetTypeName()))
					.ToArray());

			var componentTypeToIndexLookup = string.Join(
				",\n",
				data
					.Select(
						(d, index) => COMPONENT_TYPE_TO_INDEX_TEMPLATE
							.Replace("${ComponentType}", d.GetTypeName())
							.Replace("${Index}", index.ToString()))
					.ToArray());


			var fileContent = TEMPLATE
				.Replace("${Lookup}", k_LookUpTableClassName)
				.Replace("${componentConstantsList}", componentConstantsList)
				.Replace("${totalComponentsConstant}", totalComponentsConstant)
				.Replace("${componentNamesList}", componentNamesList)
				.Replace("${componentTypesList}", componentTypesList)
				.Replace("${componentTypeToIndexLookup}", componentTypeToIndexLookup);

			return new CodeGenFile(
				"Item" +
				Path.DirectorySeparatorChar +
				"PowerupsLookup.cs",
				fileContent,
				GetType().FullName);
		}
	}
}
*/