namespace Models.Settings
{
    public class SwaggerDataProviderSettings : DataProviderSettings
    {
        public bool UseLocalDataSource { get; set; }
        
        public string LocalDataSource { get; set; }

        // Treat all non-specified properties as nullable
        public bool NonSpecifiedPropertiesAreNullable { get; set; } = false;
    }
}