using Contracts;

namespace Domain.Models.General
{
	public class GroupModel : IGroupProviderModel
	{
		public List<string> Imports { get; set; }
		public string Namespace { get; set; }
		public string Schema { get; set; }
		public IEnumerable<IProviderModel> Models { get; set; }
	}
}