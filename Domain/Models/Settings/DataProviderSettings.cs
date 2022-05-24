using Contracts;
using Domain.Enums;

namespace Models.Settings
{
	public class DataProviderSettings : IDataProviderSettings
	{
		public string DataSource { get; set; }
		
		public string Name { get; set; }

		public DataProviderTypes TypeName { get; set; }
	}
}