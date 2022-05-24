using Contracts;
using Domain.Models.General;
using Models;

namespace Engine.Models
{
	public class Model : ModelBase, IProviderModel
	{
		public List<Property> Properties { get; set; } = new List<Property>();
	}
}