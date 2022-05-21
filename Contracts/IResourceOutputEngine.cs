using Models;

namespace Contracts
{
	public interface IResourceOutputEngine
	{
		OperationResult Write(GenerationContext context);
	}
}