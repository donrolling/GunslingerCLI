using Domain.Enums;

namespace Contracts
{
	public interface IDataProviderSettings
	{
		public string DataSource { get; }

		public string Name { get; }

		public DataProviderTypes TypeName { get; }
	}
}