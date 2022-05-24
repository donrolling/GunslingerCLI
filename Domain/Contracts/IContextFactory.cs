using Domain.Models;
using Models;

namespace Contracts
{
	public interface IContextFactory
    {
        OperationResult<GenerationContext> Create(CommandSettings commandSettings);
    }
}