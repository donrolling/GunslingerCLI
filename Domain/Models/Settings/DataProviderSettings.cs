using Contracts;
using Domain.Enums;

namespace Domain.Models.Settings
{
	public class DataProviderSettings : IDataProviderSettings
	{
		public string DataSource { get; set; }

		public string Name { get; set; }

		public DataProviderTypes TypeName { get; set; }
	}
}