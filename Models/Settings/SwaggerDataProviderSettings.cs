namespace Models.Settings
{
    public class SwaggerDataProviderSettings : DataProviderSettings
    {
        public bool UseLocalDataSource { get; set; }
        public string LocalDataSource { get; set; }
        // This allows us to bypass issues with auth on some endpoints
        public bool OpenDataSourceUrlInDefaultBrowser { get; set; } = false;
        // Treat all non-specified properties as nullable
        public bool NonSpecifiedPropertiesAreNullable { get; set; } = false;
    }
}