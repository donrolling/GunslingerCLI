using Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Models.BaseClasses;
using Models.Enums;
using Utilities.IO;

namespace Gunslinger.Factories
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
            var generationContextReadResult = FileUtility.ReadFileAsType<GenerationContext>(configPath);
            if (generationContextReadResult.Failed)
            {
                // log failure and stop
                return OperationResult.Fail<GenerationContext>("Could not find configuration file.", Status.Cancelled);
            }
            var generationContext = generationContextReadResult.Result;
            var path = Directory.GetCurrentDirectory();
            //if the user set it to a full path, let them use it, otherwise it is a relative path
            if (!generationContext.TemplateDirectory.Contains(":\\"))
            {
                generationContext.TemplateDirectory = $"{path}\\{generationContext.TemplateDirectory}";
            }
            foreach (var template in generationContext.Templates)
            {
                var templatePath = $"{generationContext.TemplateDirectory}\\{template.InputRelativePath}";
                var text = _fileProvider.Get(templatePath);
                if (string.IsNullOrEmpty(text))
                {
                    throw new Exception($"Template was empty {template.Name}");
                }
                template.TemplateText = text;
            }

            return OperationResult.Ok(generationContext);
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