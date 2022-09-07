using EntitasRedux.Core.Plugins;
using Genesis.Plugin;
using Genesis.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitasReduxCodeGeneratorPlugin.Item.DataProviders
{/*
	internal sealed class CreatePowerupMethodDataProvider : IDataProvider, IConfigurable, ICacheable
	{
		public string Name => "Create Powerup Method";

		public int Priority => 0;

		public bool RunInDryMode => true;

		private IMemoryCache _memoryCache;
		private AssembliesConfig _assembliesConfig;

		public void SetCache(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}
		public void Configure(IGenesisConfig genesisConfig)
		{
			_assembliesConfig = genesisConfig.CreateAndConfigure<AssembliesConfig>();
		}

		public CodeGeneratorData[] GetData()
		{
			var namedTypeSymbols =
				_assembliesConfig.FilterTypeSymbols(_memoryCache.GetNamedTypeSymbols());

			var createPowerupMethodData = namedTypeSymbols
				.Where(cachedNamedTypeSymbol => cachedNamedTypeSymbol.NamedTypeSymbol.IsStatic)
				.SelectMany(getCreatePowerupMethodsFromNamedTypeSymbol);

			throw new NotImplementedException();
		}

		private MethodData[] getCreatePowerupMethodsFromNamedTypeSymbol(ICachedNamedTypeSymbol cachedNamedTypeSymbol)
		{
			throw new NotImplementedException();
		}
	}*/
}
