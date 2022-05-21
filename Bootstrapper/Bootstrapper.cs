using Contracts;
using Engine.Engines;
using Engine.Services;
using Gunslinger.DataProviders;
using Gunslinger.Engines;
using Gunslinger.Factories;
using Gunslinger.Factories.SQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Models.Constants;
using Serilog;

namespace Bootstrapper
{
	public class Configuration
	{
		public static IHost ConfigureServices()
		{
			Log.Logger = new LoggerConfiguration().CreateLogger();

			return Host.CreateDefaultBuilder()
				.ConfigureAppConfiguration((context, builder) =>
				{
					builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddOptions<AppSettings>().Bind(hostContext.Configuration.GetSection(ConfigKeys.AppSettings)).ValidateDataAnnotations();
					services.AddTransient<IGeneratorService, GeneratorService>();
					services.AddTransient<IModelGeneratorService, ModelGeneratorService>();
					services.AddTransient<ITemplateOutputEngine, FileOutputEngine>();
					services.AddTransient<IResourceOutputEngine, ResourceOutputEngine>();
					services.AddTransient<IFileProvider, FileTemplateProvider>();
					services.AddTransient<IRenderEngine, HandlebarsRenderEngine>();
					//services.AddTransient<IContextFactory, GenerationContextFactory>();
					services.AddTransient<IDataProviderFactory, DataProviderFactory>();
					services.AddTransient<ISQLServerInfoFactory, SQLServerInfoFactory>();
				})
				.UseSerilog((hostContext, loggerConfiguration) =>
				{
					loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
				})
			   .Build();
		}
	}
}