namespace Contracts
{
	public interface IDataProviderFactory
	{
		IDataProvider Create(dynamic dataProviderSettings);

		IDataProvider Get(string name);
	}
}