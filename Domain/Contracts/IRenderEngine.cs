using Domain.Models.General;

namespace Contracts
{
	public interface IRenderEngine
	{
		string Render(Template template, IProviderModel model);

		string Render(Template template, IGroupProviderModel model);
	}
}