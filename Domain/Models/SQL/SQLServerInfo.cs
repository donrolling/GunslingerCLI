using Microsoft.SqlServer.Management.Smo;

namespace Domain.Models.SQL
{
	public class SQLServerInfo
	{
		public string DatabaseName { get; set; }
		public string ServerName { get; set; }
		public Server Server { get; set; }
		public Database Database { get; set; }
	}
}