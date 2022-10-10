using Contracts;
using Domain.Models.BaseClasses;
using Domain.Models.SQL;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Engine.Factories.SQL
{
	public class SQLServerInfoFactory : LoggingWorker, ISQLServerInfoFactory
	{
		private static readonly Dictionary<string, SQLServerInfo> _sqlServerInfo;

		static SQLServerInfoFactory()
		{
			_sqlServerInfo = new Dictionary<string, SQLServerInfo>();
		}

		public SQLServerInfoFactory(ILoggerFactory loggerFactory) : base(loggerFactory)
		{
		}

		/// <summary>
		/// Creates SMO objects for schema inspection
		/// </summary>
		/// <param name="name"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public SQLServerInfo Create(string name, string connectionString)
		{
			//don't construct this stuff twice
			if (_sqlServerInfo.ContainsKey(name))
			{
				return _sqlServerInfo[name];
			}

			var builder = new SqlConnectionStringBuilder(connectionString);
			var sqlConnection = new SqlConnection(connectionString);
			var serverConnection = new ServerConnection(sqlConnection);
			var server = new Server(serverConnection);
			var sqlServerInfo = new SQLServerInfo
			{
				DatabaseName = builder.InitialCatalog,
				Server = server,
				ServerName = builder.DataSource,
			};
			sqlServerInfo.Database = new Database(server, sqlServerInfo.DatabaseName);

			//add it to the list
			_sqlServerInfo.Add(name, sqlServerInfo);

			return sqlServerInfo;
		}
	}
}