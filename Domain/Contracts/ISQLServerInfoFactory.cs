using Domain.Models.SQL;

namespace Contracts
{
	public interface ISQLServerInfoFactory
	{
		SQLServerInfo Create(string name, string connectionString);
	}
}