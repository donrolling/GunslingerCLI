namespace Contracts
{
	public interface IDataProviderSettings
	{
		public string DataSource { get; set; }

		public string Name { get; set; }

		public string TypeName { get; set; }
	}
}