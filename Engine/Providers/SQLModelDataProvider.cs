using Contracts;
using Domain.Enums;
using Domain.Models;
using Engine.Models.SQL;
using Gunslinger.Factories.SQL;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;
using Models;
using Models.BaseClasses;
using Models.Settings;

namespace Gunslinger.DataProviders
{
	public class SQLModelDataProvider : LoggingWorker, IDataProvider
	{
		private readonly SQLDataProviderSettings _dataProviderSettings;
		private readonly ISQLServerInfoFactory _sqlServerInfoFactory;

		public DataProviderTypes TypeName
		{
			get { return _dataProviderSettings.TypeName; }
		}

		public string DataSource
		{
			get { return _dataProviderSettings.DataSource; }
		}

		public string Name
		{
			get { return _dataProviderSettings.Name; }
		}

		public SQLModelDataProvider(
			ISQLServerInfoFactory sqlServerInfoFactory, 
			SQLDataProviderSettings dataProvider, 
			ILoggerFactory loggerFactory
		) : base(loggerFactory)
		{
			_dataProviderSettings = dataProvider;
			_sqlServerInfoFactory = sqlServerInfoFactory;
		}

		public OperationResult<Dictionary<string, IProviderModel>> Get(
			GenerationContext context, 
			Template template, 
			List<string> includeTheseEntitiesOnly, 
			List<string> excludeTheseEntities
		)
		{
			try
			{
				//this call won't recreate the SQLServerInfo over multiple calls
				var sqlServerInfo = _sqlServerInfoFactory.Create(_dataProviderSettings.Name, _dataProviderSettings.DataSource);
				sqlServerInfo.Database.Refresh();
				var smoTables = TableInfoFactory.Create(sqlServerInfo, context, includeTheseEntitiesOnly, excludeTheseEntities);
				var gunslingerTables = SQLTableFactory.Create(template.Namespace, template.Language, smoTables, template);
				var smoViews = _dataProviderSettings.GenerateViews
					? ViewInfoFactory.Create(sqlServerInfo, context, includeTheseEntitiesOnly, excludeTheseEntities)
					: new List<View>();
				var gunslingerViews = _dataProviderSettings.GenerateViews
					? SQLViewFactory.Create(template.Namespace, template.Language, smoViews, template)
					: new List<SQLView>();
				var providerModels = new Dictionary<string, IProviderModel>();
				foreach (var gunslingerTable in gunslingerTables)
				{
					var sqlModel = SQLModelFactory.Create(gunslingerTable, context);
					providerModels.Add(gunslingerTable.UniqueName, sqlModel);
				}
				return OperationResult.Ok(providerModels);
			}
			catch (Exception ex)
			{
				return OperationResult.Fail<Dictionary<string, IProviderModel>>($"SQL Database Data Provider had a failure: {ex.Message}\r\n{ex.StackTrace}");
			}
		}
	}
}