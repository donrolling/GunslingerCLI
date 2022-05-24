using Contracts;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Models.BaseClasses;
using Models.Enums;

namespace Engine.Services
{
    public class GeneratorService : LoggingWorker, IGeneratorService
    {
        public GenerationContext Context { get; private set; }

        private readonly IContextFactory _contextFactory;
        private readonly IDataProviderFactory _dataProviderFactory;
        private readonly IModelGeneratorService _modelGeneratorService;
        private readonly IResourceOutputEngine _resourceOutputEngine;
        private readonly AppSettings _appSettings;

        public GeneratorService(
            IContextFactory contextFactory,
            IDataProviderFactory dataProviderFactory,
            IModelGeneratorService modelGeneratorService,
            IResourceOutputEngine resourceOutputEngine,
            ILoggerFactory loggerFactory,
            IOptions<AppSettings> appSettings
        ) : base(loggerFactory)
        {
            _contextFactory = contextFactory;
            _dataProviderFactory = dataProviderFactory;
            _modelGeneratorService = modelGeneratorService;
            _resourceOutputEngine = resourceOutputEngine;
            _appSettings = appSettings.Value;
        }

        public OperationResult Generate(CommandSettings commandSettings)
        {
            var contextResult = _contextFactory.Create(commandSettings);
            if (contextResult.Failed)
            {
                return contextResult;
            }
            Context = contextResult.Result;

            var errors = new List<OperationResult>();

            // initialize all data providers
            var dataProviderNames = Context.Templates.Select(a => a.DataProviderName).Distinct();
            foreach (var dataProviderName in dataProviderNames)
            {
                var dataProviderDefinition = Context.DataProviders.First(a => a.Name == dataProviderName);
                _dataProviderFactory.Create(dataProviderDefinition);
            }

            foreach (var template in Context.Templates)
            {
                switch (template.Type)
                {
                    case TemplateType.Model:
                        var generateResult = _modelGeneratorService.GenerateMany(Context, template);
                        if (generateResult.Failed)
                        {
                            errors.Add(generateResult);
                            Logger.LogError(generateResult.Message);
                        }

                        break;

                    case TemplateType.Setup:
                    default:
                        var generateOneResult = _modelGeneratorService.GenerateOne(Context, template);
                        if (generateOneResult.Failed)
                        {
                            errors.Add(generateOneResult);
                            Logger.LogError(generateOneResult.Message);
                        }

                        break;
                }
            }

            // copy resource files
            var resourseWriteResult = _resourceOutputEngine.Write(Context);
            if (resourseWriteResult.Failed)
            {
                return resourseWriteResult;
            }

            // done
            if (errors.Any())
            {
                var message = errors.Select(a => a.Message).Aggregate("Generation was not completely successful.\r\n\t", (accumulator, next) => $"{accumulator}\r\n\t{next}");
                return OperationResult.Fail(message);
            }
            return OperationResult.Ok();
        }
    }
}