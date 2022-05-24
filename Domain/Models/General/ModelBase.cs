using Domain.Models.General;

namespace Models
{
	public class ModelBase
	{
		public string Description { get; set; }
		public List<string> Imports { get; set; }
		public Name Name { get; set; }
		public string Namespace { get; set; }
		public string Schema { get; set; }
		public string Type { get; set; }
	}
}