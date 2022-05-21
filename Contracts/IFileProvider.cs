using Models;

namespace Contracts
{
	public interface IFileProvider
	{
		string Get(GenerationContext context, string filename);

		string Get(string filename);
	}
}