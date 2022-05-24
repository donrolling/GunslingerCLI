using Models;

namespace Contracts
{
	public interface IRenderEngine
	{
		string Render(Template template, IProviderModel model);

		string Render(Template template, IGroupProviderModel model);
	}
}