using Domain.Models;
using Domain.Models.General;

namespace Contracts
{
	public interface IModelGeneratorService
	{
		OperationResult Generate(GenerationContext context, IEnumerable<Template> templates);

		OperationResult GenerateMany(GenerationContext context, Template template);

		OperationResult GenerateOne(GenerationContext context, Template template);
	}
}