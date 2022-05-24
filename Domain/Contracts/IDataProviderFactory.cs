namespace Contracts
{
	public interface IDataProviderFactory
	{
		IDataProvider Create(IDataProviderSettings dataProviderSettings);

		IDataProvider Get(string name);
	}
}