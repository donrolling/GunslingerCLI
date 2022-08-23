using Contracts;
using Domain.Models.Configuration;
using Engine.Factories.SQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tests.Models;
using Tests.TemplateSetTests;
using Tests.TestExtensions;

namespace Tests.Tests
{
	[TestClass]
	public class ProcInfoFactoryTests : BaseTests
	{
		private ISQLServerInfoFactory _sqlServerInfoFactory;

		public TestContext TestContext { get; set; }

		public ProcInfoFactoryTests()
		{
			_sqlServerInfoFactory = GetService<ISQLServerInfoFactory>();
		}

		[TestMethod]
		public void GetAllStoredProcedures()
		{
			var testConnection = TestContext.ReadJSON<TestConnection>("settings.json");
			var sqlServerInfo = _sqlServerInfoFactory.Create("Test", testConnection.ConnectionString);
			var settings = new GenerationContext();
			var includeTheseEntitiesOnly = new List<string>();
			var excludeTheseEntities = new List<string>();
			var xs = ProcInfoFactory.Create(sqlServerInfo, settings, includeTheseEntitiesOnly, excludeTheseEntities);
			Assert.IsNotNull(xs);
		}
	}
}