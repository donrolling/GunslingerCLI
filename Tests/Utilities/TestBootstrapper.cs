using Bootstrapper;
using Contracts;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Linq;

namespace Tests.Utilities
{
	public static class TestBootstrapper
    {
        public static GenerationContext GenerationContext { get; private set; }
        public static ServiceProvider ServiceProvider { get; private set; }

        public static IServiceProvider Bootstrap(string pathToGenerationContext)
        {
            GenerationContext = GenerationConfiguration.GetGenerationConfiguration(pathToGenerationContext);
            return ServiceProvider;
        }

        public static IGeneratorService GetGeneratorService(string relativePathToGenerationContext)
        {
            var serviceProvider = Bootstrap(relativePathToGenerationContext);
            var generatorService = serviceProvider.GetService<IGeneratorService>();

            // make sure that the OutputDirectory goes to root\\Output\\whatever
            var directories = AppDomain.CurrentDomain.BaseDirectory.Split("\\bin\\")[0].Split("\\");
            var baseDirectory = string.Join('\\', directories.Take(directories.Length - 1));
            generatorService.Context.OutputDirectory = $"{baseDirectory}\\{generatorService.Context.OutputDirectory}";
            //foreach (var dataProvider in generatorService.Context.DataProviders)
            //{
            //    if (!string.IsNullOrEmpty(dataProvider["LocalDataSource"].Value))
            //    {
            //        dataProvider.LocalDataSource = $"{baseDirectory}\\Output\\{dataProvider.LocalDataSource}";
            //    }
            //}
            return generatorService;
        }
    }
}