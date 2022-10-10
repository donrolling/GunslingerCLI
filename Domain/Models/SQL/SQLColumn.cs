using Microsoft.SqlServer.Management.Smo;

namespace Domain.Models.SQL
{
	public class SQLColumn : General.Property
	{
		public bool IsInPrimaryKey { get; set; }
		public Guid Id { get; set; } = Guid.NewGuid();
		public bool PrimaryKey { get; set; }
		public SqlDataType SqlDataTypeEnum { get; set; }
		public string SqlDataType { get; set; }
		public object DefaultValue { get; set; }
		public int Length { get; set; }
		public bool IsForeignKey { get; set; }
	}
}