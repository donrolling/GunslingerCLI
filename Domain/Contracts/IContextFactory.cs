using Domain.Models.Configuration;
using Domain.Models.General;

namespace Contracts
{
	public interface IContextFactory
	{
		OperationResult<GenerationContext> Create(CommandSettings commandSettings);
	}
}