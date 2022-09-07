using Genesis.Plugin;
using Genesis.Shared;
using EntitasRedux.Core.Plugins;
using DYEntitasRedux;
using Serilog;
using System.Linq;

namespace DYEntitasRedux.Core.Plugins
{
	internal sealed class ConsumableComponentDataProvider : IDataProvider,
															IConfigurable,
															ICacheable
	{
		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private readonly ILogger _logger;
		private readonly AssembliesConfig _assembliesConfig;

		private IMemoryCache _memoryCache;

		private const string NAME = "Component";

		public ConsumableComponentDataProvider()
		{
		}

		public CodeGeneratorData[] GetData()
		{
			var namedTypeSymbols =
				_assembliesConfig.FilterTypeSymbols(_memoryCache.GetNamedTypeSymbols());
			var dontGenerateAttributeName = nameof(JCMG.EntitasRedux.DontGenerateAttribute);
			var generateNamedTypeSymbols = namedTypeSymbols
				.Where(namedTypeSymbol => !namedTypeSymbol.HasAttribute(dontGenerateAttributeName));

			var dataFromComponents = generateNamedTypeSymbols
				.Where(cachedNamedTypeSymbol => cachedNamedTypeSymbol.ImplementsInterface<JCMG.EntitasRedux.IComponent>())
				.Where(cachedNamedTypeSymbol => cachedNamedTypeSymbol.NamedTypeSymbol.InheritsFrom<Consumable>())
				.Where(cachedNamedTypeSymbol => !cachedNamedTypeSymbol.NamedTypeSymbol.IsAbstract)
				.SelectMany(CreateDataForComponents)
				.ToArray();

			var dataFromEvents = dataFromComponents
				.Where(data => data.IsEvent())
				.SelectMany(CreateDataForEvents)
				.ToArray();

			var finalMergedData = Merge(dataFromEvents, dataFromComponents).ToArray();

			ComponentValidationTools.ValidateComponentData(_logger, finalMergedData);

			return finalMergedData;
		}

		/// <inheritdoc />
		public void SetCache(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		private ComponentData[] Merge(ComponentData[] priorData, ComponentData[] redundantData)
		{
			return redundantData
				.Concat(priorData)
				.Distinct(new ComponentDataEqualityComparer())
				.ToArray();
		}

		private ComponentData[] CreateDataForComponents(ICachedNamedTypeSymbol cachedNamedTypeSymbol)
		{
			return GetComponentNames(cachedNamedTypeSymbol)
				.Select(
					componentName =>
					{
						var data = CreateDataForComponent(cachedNamedTypeSymbol);
						return data;
					})
				.ToArray();
		}

		private ComponentData CreateDataForComponent(ICachedNamedTypeSymbol namedTypeSymbol)
		{
			var data = new ComponentData();

			/*
			foreach (var provider in _dataProviders)
			{
				provider.Provide(namedTypeSymbol, data);
			}
			*/
			return data;
		}

		private ComponentData[] CreateDataForEvents(ComponentData data)
		{
			return data.GetContextNames()
				.SelectMany(
					contextName =>
						data.GetEventData()
							.Select(
								eventData =>
								{
									var dataForEvent = new ComponentData(data);
									dataForEvent.IsEvent(false);
									dataForEvent.IsUnique(false);
									dataForEvent.ShouldGenerateComponent(false);
									dataForEvent.RemoveCleanupData();
									var eventComponentName = data.EventComponentName(eventData);
									var eventTypeSuffix = eventData.GetEventTypeSuffix();
									var optionalContextName =
										dataForEvent.GetContextNames().Length > 1 ? contextName : string.Empty;
									var listenerComponentName =
										optionalContextName + eventComponentName + eventTypeSuffix.AddListenerSuffix();
									dataForEvent.SetTypeName(listenerComponentName.AddComponentSuffix());
									dataForEvent.SetMemberData(
										new[]
										{
											new MemberData(
												"System.Collections.Generic.List<I" + listenerComponentName + ">",
												"value")
										});
									dataForEvent.SetContextNames(
										new[]
										{
											contextName
										});
									return dataForEvent;
								})
							.ToArray())
				.ToArray();
		}

		private bool HasContexts(ICachedNamedTypeSymbol cachedNamedTypeSymbol)
		{
			return cachedNamedTypeSymbol.NamedTypeSymbol.GetContextNames().Length != 0;
		}

		private string[] GetComponentNames(ICachedNamedTypeSymbol namedTypeSymbol)
		{
			// if there are not any
			var componentAttrTypeSymbols = namedTypeSymbol.GetAttributes(nameof(JCMG.EntitasRedux.ComponentNameAttribute));
			if (!componentAttrTypeSymbols.Any())
			{
				var componentNames = new[]
				{
					namedTypeSymbol.TypeName
				};

				return componentNames;
			}

			return componentAttrTypeSymbols
				.SelectMany(x => x.ConstructorArguments)
				.SelectMany(x => x.Values)
				.Select(x => x.Value.ToString())
				.ToArray();
		}

		public void Configure(IGenesisConfig genesisConfig)
		{
			_assembliesConfig.Configure(genesisConfig);
		}
	}
}
