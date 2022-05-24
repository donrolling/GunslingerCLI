using Domain.Models;
using Domain.Models.General;
using Models;

namespace Contracts
{
	public interface IContextFactory
    {
        OperationResult<GenerationContext> Create(CommandSettings commandSettings);
    }
}