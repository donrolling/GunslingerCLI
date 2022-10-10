using Contracts;
using Domain.Models.Configuration;
using Domain.Models.General;
using Domain.Models.Settings;
using Engine.Factories;
using Engine.Factories.SQL;
using Gunslinger.DataProviders;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Models;
using Tests.TemplateSetTests;
using Tests.TestExtensions;

namespace Tests.Tests
{
	[TestClass]
	public class ProcInfoFactoryTests : BaseTests
	{
		private ISQLServerInfoFactory _sqlServerInfoFactory;
		private readonly IContextFactory _contextFactory;

		public TestContext TestContext { get; set; }

		public ProcInfoFactoryTests()
		{
			_sqlServerInfoFactory = GetService<ISQLServerInfoFactory>();
			_contextFactory = GetService<IContextFactory>();
		}

		[TestMethod]
		public void GetAllStoredProcedures()
		{
			var testConnection = TestContext.ReadJSON<TestConnection>("settings.json");
			var sqlServerInfo = _sqlServerInfoFactory.Create(Guid.NewGuid().ToString(), testConnection.ConnectionString);
			var context = new GenerationContext();
			var includeTheseEntitiesOnly = new List<string>();
			var excludeTheseEntities = new List<string>();
			var xs = ProcInfoFactory.Create(sqlServerInfo, context, includeTheseEntitiesOnly, excludeTheseEntities);
			Assert.IsNotNull(xs);
			Assert.IsTrue(xs.Any());
		}

		[TestMethod]
		public void GetStoredProceduresAsCustomObjects()
		{
			var commandSettings = this.GetCommandSettings(TestContext, "gunslinger.json");
			var context = _contextFactory.Create(commandSettings);
			var dataProviderSettings = (SQLDataProviderSettings)context.Result.DataProviders.First();
			var loggerFactory = GetService<ILoggerFactory>();
			var procDataProvider = new SQLProcDataProvider(_sqlServerInfoFactory, dataProviderSettings, loggerFactory);
			var template = new Template();
			var includeTheseEntitiesOnly = new List<string>();
			var excludeTheseEntities = new List<string>();
			var xs = procDataProvider.Get(context.Result, template, includeTheseEntitiesOnly, excludeTheseEntities);
			Assert.IsNotNull(xs);
			Assert.IsTrue(xs.Success);
			Assert.IsTrue(xs.Result.Any());
		}
	}
}