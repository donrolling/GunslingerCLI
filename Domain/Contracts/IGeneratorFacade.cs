using Domain.Models.Configuration;
using Domain.Models.General;

namespace Contracts
{
	public interface IGeneratorService
	{
		GenerationContext Context { get; }

		OperationResult Generate(CommandSettings commandSettings);
	}
}