﻿using Contracts;
using Domain.Enums;
using Domain.Models.BaseClasses;
using Domain.Models.Configuration;
using Domain.Models.General;
using Domain.Models.Settings;
using Engine.Factories.SQL;
using Microsoft.Extensions.Logging;

namespace Gunslinger.DataProviders
{
	public class SQLProcDataProvider : LoggingWorker, IDataProvider
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

		public SQLProcDataProvider(
			ISQLServerInfoFactory sqlServerInfoFactory,
			SQLDataProviderSettings dataProviderSettings,
			ILoggerFactory loggerFactory
		) : base(loggerFactory)
		{
			_dataProviderSettings = dataProviderSettings;
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
				var smoProcs = ProcInfoFactory.Create(sqlServerInfo, context, includeTheseEntitiesOnly, excludeTheseEntities);
				var gunslingerProcs = SQLProcFactory.Create(template.Namespace, template.Language, smoProcs, template);
				var providerModels = new Dictionary<string, IProviderModel>();
				foreach (var gunslingerProc in gunslingerProcs)
				{
					providerModels.Add(gunslingerProc.UniqueName, gunslingerProc);
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