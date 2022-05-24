using Domain.Models;
using Models;

namespace Contracts
{
	public interface IGeneratorService
    {
        GenerationContext Context { get; }

        OperationResult Generate(CommandSettings commandSettings);
    }
}