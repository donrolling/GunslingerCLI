using Contracts;
using Domain.Constants;
using Engine.Engines;
using Engine.Factories;
using Engine.Factories.SQL;
using Engine.Services;
using Gunslinger.DataProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Serilog;

namespace Bootstrapper
{
	public class Configuration
	{
		public static IHost ConfigureServices(string baseDirectory = "")
		{
			Log.Logger = new LoggerConfiguration().CreateLogger();

			return Host.CreateDefaultBuilder()
				.ConfigureAppConfiguration((context, builder) =>
				{
					var path = $"{baseDirectory}appsettings.json";
					builder.AddJsonFile(path, optional: false, reloadOnChange: true);
				})
				.ConfigureServices((hostContext, services) =>
				{
					// settings
					services.AddOptions<AppSettings>().Bind(hostContext.Configuration.GetSection(ConfigKeys.AppSettings)).ValidateDataAnnotations();

					// providers
					services.AddTransient<IFileProvider, FileTemplateProvider>();

					// services
					services.AddTransient<IGeneratorService, GeneratorService>();
					services.AddTransient<IModelGeneratorService, ModelGeneratorService>();

					// engines
					services.AddTransient<ITemplateOutputEngine, FileOutputEngine>();
					services.AddTransient<IResourceOutputEngine, ResourceOutputEngine>();
					services.AddTransient<IRenderEngine, HandlebarsRenderEngine>();

					// factories
					services.AddTransient<IContextFactory, GenerationContextFactory>();
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