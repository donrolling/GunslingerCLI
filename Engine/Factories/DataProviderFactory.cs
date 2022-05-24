using Contracts;
using Domain.Models.Settings;
using Gunslinger.DataProviders;
using Microsoft.Extensions.Logging;
using Models.BaseClasses;
using Newtonsoft.Json;

namespace Gunslinger.Factories
{
	public class DataProviderFactory : LoggingWorker, IDataProviderFactory
	{
		private static readonly Dictionary<string, IDataProvider> _dataProviderDictionary;

		private readonly ILoggerFactory _loggerFactory;
		private readonly ISQLServerInfoFactory _sqlServerInfoFactory;

		static DataProviderFactory()
		{
			_dataProviderDictionary = new Dictionary<string, IDataProvider>();
		}

		public DataProviderFactory(ISQLServerInfoFactory sqlServerInfoFactory, ILoggerFactory loggerFactory) : base(loggerFactory)
		{
			_sqlServerInfoFactory = sqlServerInfoFactory;
			_loggerFactory = loggerFactory;
		}

		public IDataProvider Create(IDataProviderSettings dataProviderSettings)
		{
			if (_dataProviderDictionary.ContainsKey(dataProviderSettings.Name))
			{
				return _dataProviderDictionary[dataProviderSettings.Name];
			}
			switch (dataProviderSettings.TypeName)
			{
				case Domain.Enums.DataProviderTypes.SwaggerDataProvider:
					var swaggerDataProviderSettings = JsonConvert.DeserializeObject<SwaggerDataProviderSettings>(JsonConvert.SerializeObject(dataProviderSettings));
					var swaggerDataProvider = new SwaggerDataProvider(swaggerDataProviderSettings, _loggerFactory);
					_dataProviderDictionary.Add(swaggerDataProviderSettings.Name, swaggerDataProvider);
					return swaggerDataProvider;

				case Domain.Enums.DataProviderTypes.SQLDataProvider:
					var sqlDataProviderSettings = JsonConvert.DeserializeObject<SQLDataProviderSettings>(JsonConvert.SerializeObject(dataProviderSettings));
					var sqlModelDataProvider = new SQLModelDataProvider(_sqlServerInfoFactory, sqlDataProviderSettings, _loggerFactory);
					_dataProviderDictionary.Add(sqlDataProviderSettings.Name, sqlModelDataProvider);
					return sqlModelDataProvider;

				default:
					var msg = $"Create() - Name not matched: {dataProviderSettings.TypeName}";
					this.Logger.LogError(msg);
					throw new Exception(msg);
			}
		}

		public IDataProvider Get(string name)
		{
			if (_dataProviderDictionary.ContainsKey(name))
			{
				return _dataProviderDictionary[name];
			}
			else
			{
				var msg = $"Get() - Name not matched: {name}";
				this.Logger.LogError(msg);
				throw new Exception(msg);
			}
		}
	}
}