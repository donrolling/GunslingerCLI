using Contracts;
using Models;

namespace Domain.Models.General
{
	public class Model : ModelBase, IProviderModel
	{
		public List<Property> Properties { get; set; } = new List<Property>();
	}
}