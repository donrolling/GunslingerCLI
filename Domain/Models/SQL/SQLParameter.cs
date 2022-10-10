namespace Domain.Models.SQL
{
	public class SQLParameter : General.Parameter
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string SqlDataType { get; set; }
		public object DefaultValue { get; set; }
		public int Length { get; set; }
	}
}