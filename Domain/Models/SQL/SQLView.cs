using Contracts;
using Models;

namespace Domain.Models.SQL
{
	public class SQLView : ModelBase, IProviderModel
	{
		public string UniqueName { get; set; }
		public List<SQLKey> Keys { get; set; }
		public List<SQLColumn> Columns { get; set; }
	}
}