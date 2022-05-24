using Contracts;
using Models;

namespace Engine.Models
{
	public class Model : ModelBase, IProviderModel
	{
		public List<Property> Properties { get; set; } = new List<Property>();
	}
}