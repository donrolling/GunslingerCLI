namespace Models.SQL
{
	public class SQLKey
	{
		public string DataType { get; set; }
		public string DbType { get; set; }
		public Name Name { get; set; }
		public bool IsNullable { get; set; }
		public string SQLDataType { get; set; }
	}
}