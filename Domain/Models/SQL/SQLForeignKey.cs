namespace Domain.Models.SQL
{
	public class SQLForeignKey
	{
		public bool IsNullable { get; set; }
		public ColumnSource Source { get; set; }
		public ColumnSource Reference { get; set; }
	}
}