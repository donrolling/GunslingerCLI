using Contracts;
using Domain.Enums;
using Domain.Models;
using Domain.Models.BaseClasses;
using Domain.Models.General;
using Domain.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Utilities.IO;

namespace Engine.Factories
{
	public class GenerationContextFactory : LoggingWorker, IContextFactory
	{
		private readonly AppSettings _appSettings;
		private readonly IFileProvider _fileProvider;

		public GenerationContextFactory(
			IOptions<AppSettings> appSettings,
			IFileProvider fileProvider,
			ILoggerFactory loggerFactory
		) : base(loggerFactory)
		{
			_appSettings = appSettings.Value;
			_fileProvider = fileProvider;
		}

		public OperationResult<GenerationContext> Create(CommandSettings commandSettings)
		{
			var configPath = GetConfigurationPath(commandSettings);
			var generationContextReadResult = FileUtility.ReadFileAsType<GenerationContextJSON>(configPath);
			if (generationContextReadResult.Failed)
			{
				// log failure and stop
				var msg = $"Could not find configuration file. Searched here: {configPath}.{Environment.NewLine}Type 'gs --help' for help.";
				return OperationResult.Fail<GenerationContext>(msg, Status.Cancelled);
			}
			var generationContextJSON = generationContextReadResult.Result;
			var generationContext = new GenerationContext
			{
				AuditProperties = generationContextJSON.AuditProperties,
				ExcludeTheseEntities = generationContextJSON.ExcludeTheseEntities,
				ExcludeTheseTemplates = generationContextJSON.ExcludeTheseTemplates,
				IncludeTheseEntitiesOnly = generationContextJSON.IncludeTheseEntitiesOnly,
				IncludeTheseTemplatesOnly = generationContextJSON.IncludeTheseTemplatesOnly,
				OutputDirectory = FixPath(commandSettings.RootPath, generationContextJSON.OutputDirectory),
				ProcessTemplateStubs = generationContextJSON.ProcessTemplateStubs,
				Resources = generationContextJSON.Resources,
				RootPath = commandSettings.RootPath,
				TemplateDirectory = FixPath(commandSettings.RootPath, generationContextJSON.TemplateDirectory)
			};
			ConfigureDataProviders(generationContextJSON, generationContext);
			ReadTemplateText(generationContextJSON, generationContext);
			return OperationResult.Ok(generationContext);
		}

		private void ReadTemplateText(GenerationContextJSON generationContextJSON, GenerationContext generationContext)
		{
			// read the template text for all of the templates
			foreach (var template in generationContextJSON.Templates)
			{
				var templatePath = $"{generationContext.TemplateDirectory}\\{template.InputRelativePath}";
				var text = _fileProvider.Get(templatePath);
				if (string.IsNullOrEmpty(text))
				{
					throw new Exception($"Template was empty: {template.Name}");
				}
				template.TemplateText = text;
				// now add the template to the generation context
				generationContext.Templates.Add(template);
			}
		}

		private static string FixPath(string rootPath, string path)
		{
			// if the user set either of these paths to a full path, then nothing needs to happen
			// but we need the full path, so prepend the root directory to any relative paths
			return path.Contains(":\\") ? path : $"{rootPath}\\{path}";
		}

		private void ConfigureDataProviders(GenerationContextJSON generationContextJSON, GenerationContext generationContext)
		{
			foreach (var dataProvider in generationContextJSON.DataProviders)
			{
				DataProviderTypes dataProviderType;
				var typeName = dataProvider["TypeName"].ToString();
				if (Enum.TryParse(typeName, out dataProviderType))
				{
					switch (dataProviderType)
					{
						case DataProviderTypes.SQLDataProvider:
							var sqlDataProvider = ConfigureSQLDataProvider(dataProvider);
							generationContext.DataProviders.Add(sqlDataProvider);
							break;

						case DataProviderTypes.SwaggerDataProvider:
							var swaggerDataProvider = ConfigureSwaggerDataProvider(dataProvider);
							generationContext.DataProviders.Add(swaggerDataProvider);
							break;

						default:
							break;
					}
				}
				else
				{
					throw new Exception($"Could not properly type the DataProviderType for '{dataProvider["Name"]}'");
				}
			}
		}

		private SQLDataProviderSettings ConfigureSQLDataProvider(dynamic dataProvider)
		{
			var sqlDataProviderSettings = new SQLDataProviderSettings();
			sqlDataProviderSettings.TypeName = DataProviderTypes.SQLDataProvider;
			sqlDataProviderSettings.DataSource = dataProvider[nameof(SQLDataProviderSettings.DataSource)].ToString();
			sqlDataProviderSettings.Name = dataProvider[nameof(SQLDataProviderSettings.Name)].ToString();
			var generateViews = bool.Parse(dataProvider[nameof(SQLDataProviderSettings.GenerateViews)].ToString().ToLower());
			sqlDataProviderSettings.GenerateViews = generateViews;
			return sqlDataProviderSettings;
		}

		private SwaggerDataProviderSettings ConfigureSwaggerDataProvider(dynamic dataProvider)
		{
			var swaggerDataProviderSettings = new SwaggerDataProviderSettings();
			swaggerDataProviderSettings.TypeName = DataProviderTypes.SwaggerDataProvider;
			swaggerDataProviderSettings.DataSource = dataProvider[nameof(SwaggerDataProviderSettings.DataSource)].ToString();
			swaggerDataProviderSettings.Name = dataProvider[nameof(SwaggerDataProviderSettings.Name)].ToString();
			var nonSpecifiedPropertiesAreNullable = bool.Parse(dataProvider[nameof(SwaggerDataProviderSettings.NonSpecifiedPropertiesAreNullable)].ToString().ToLower());
			swaggerDataProviderSettings.NonSpecifiedPropertiesAreNullable = nonSpecifiedPropertiesAreNullable;
			return swaggerDataProviderSettings;
		}

		private string GetConfigurationPath(CommandSettings commandSettings)
		{
			return string.IsNullOrWhiteSpace(commandSettings.ConfigPath)
				? Path.Combine(commandSettings.RootPath, _appSettings.DefaultConfigFileName)
				: commandSettings.ConfigPath.Contains(Path.DirectorySeparatorChar)
					? File.Exists(commandSettings.ConfigPath)
						? commandSettings.ConfigPath
						: Path.Combine(commandSettings.ConfigPath, _appSettings.DefaultConfigFileName)
					: Path.Combine(commandSettings.RootPath, commandSettings.ConfigPath);
		}
	}
}