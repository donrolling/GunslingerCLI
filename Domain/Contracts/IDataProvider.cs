using Domain.Models.Configuration;
using Domain.Models.General;

namespace Contracts
{
	public interface IDataProvider : IDataProviderSettings
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="template"></param>
		/// <param name="includeTheseEntitiesOnly"></param>
		/// <param name="excludeTheseEntities"></param>
		/// <returns>A dictionary of named models. The key is the model name.</returns>
		OperationResult<Dictionary<string, IProviderModel>> Get(
			GenerationContext context,
			Template template,
			List<string> includeTheseEntitiesOnly,
			List<string> excludeTheseEntities
		);
	}
}