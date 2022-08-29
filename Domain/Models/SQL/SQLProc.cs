using Contracts;
using Domain.Models.General;
using Models;

namespace Domain.Models.SQL
{
	public class SQLProc : ModelBase, IProviderModel
	{
		public Name ProcName { get; set; }
		public string UniqueName { get; set; }
		public List<SQLColumn> Inputs { get; set; }
		public IEnumerable<SQLParameter> Properties { get; set; }
	}
}