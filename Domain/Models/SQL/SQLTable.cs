using Models.SQL;

namespace Engine.Models.SQL
{
	public class SQLTable : SQLBasicTable
	{
		public List<SQLForeignKey> ForeignKeys { get; set; }
	}
}