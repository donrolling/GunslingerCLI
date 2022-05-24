namespace Domain.Models.Settings
{
	public class SwaggerDataProviderSettings : DataProviderSettings
	{
		// Treat all non-specified properties as nullable
		public bool NonSpecifiedPropertiesAreNullable { get; set; } = false;
	}
}