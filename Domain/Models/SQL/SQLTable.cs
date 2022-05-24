namespace Domain.Models.SQL
{
	public class SQLTable : SQLBasicTable
	{
		public List<SQLForeignKey> ForeignKeys { get; set; }
	}
}