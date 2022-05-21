using Contracts;
using Models;
using Models.SQL;

namespace Engine.Models.SQL
{
	public class SQLView : ModelBase, IProviderModel
	{
		public string UniqueName { get; set; }
		public List<SQLKey> Keys { get; set; }
		public List<SQLColumn> Columns { get; set; }
	}
}