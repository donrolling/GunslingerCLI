using Domain.Models;
using Domain.Models.General;
using Models;

namespace Contracts
{
	public interface IGeneratorService
    {
        GenerationContext Context { get; }

        OperationResult Generate(CommandSettings commandSettings);
    }
}