using Domain.Models.Configuration;
using Domain.Models.General;

namespace Contracts
{
	public interface IResourceOutputEngine
	{
		OperationResult Write(GenerationContext context);
	}
}