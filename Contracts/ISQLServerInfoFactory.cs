using Models.Settings;
using Models.SQL;

namespace Contracts
{
	public interface ISQLServerInfoFactory
	{
		SQLServerInfo Create(SQLDataProviderSettings dataProviderSettings);
	}
}