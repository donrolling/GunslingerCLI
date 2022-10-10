using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Models;
using Tests.TestExtensions;

namespace Tests.TemplateSetTests
{
	/// <summary>
	/// These tests will throw an exception if they fail to properly connect to sql server
	/// </summary>
	[TestClass]
	public class DatabaseConnectionTests : BaseTests
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void ConnectToDockerContainerWithPortNumber()
		{
			var testConnection = TestContext.ReadJSON<TestConnection>("settings.json");
			var sqlServerInfoFactory = this.GetService<ISQLServerInfoFactory>();
			var sqlServerInfo = sqlServerInfoFactory.Create("Test", testConnection.ConnectionString);
			sqlServerInfo.Database.Refresh();
			Assert.IsTrue(sqlServerInfo.Database.Tables.Count > 0, "Table count was zero.");
		}

		[TestMethod]
		public void ConnectToStandardSQL()
		{
			var testConnection = TestContext.ReadJSON<TestConnection>("settings.json");
			var sqlServerInfoFactory = this.GetService<ISQLServerInfoFactory>();
			var sqlServerInfo = sqlServerInfoFactory.Create("Test", testConnection.ConnectionString);
			sqlServerInfo.Database.Refresh();
			Assert.IsTrue(sqlServerInfo.Database.Tables.Count > 0, "Table count was zero.");
		}
	}
}