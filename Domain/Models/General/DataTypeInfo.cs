namespace Domain.Models.General
{
	public class DataTypeInfo
	{
		public Name Name { get; set; }
		public bool IsNullable { get; set; }
		public bool IsList { get; set; }
		public bool IsTask { get; set; }
		public string ListType { get; set; } = string.Empty;
		public Type Type { get; set; }
		public bool IsDictionary { get; set; }
		public DataTypeInfo KeyType { get; set; }
	}
}