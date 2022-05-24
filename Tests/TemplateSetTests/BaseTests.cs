using Bootstrapper;
using Contracts;
using Domain.Models.Configuration;
using Domain.Models.General;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tests.TestExtensions;

namespace Tests.TemplateSetTests
{
	public abstract class BaseTests
	{
		protected string RootPath { get; }

		protected IHost Host { get; }

		public BaseTests()
		{
			RootPath = Environment.CurrentDirectory;
			Host = Configuration.ConfigureServices();
		}

		protected T GetService<T>()
		{
			return Host.Services.GetRequiredService<T>();
		}

		protected OperationResult RunGeneratorFromConfig<T>(T testClass, TestContext testContext, string filename)
		{
			using (var scope = Host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var logger = services.GetRequiredService<ILogger<T>>();
				var generatorService = services.GetRequiredService<IGeneratorService>();
				var commandSettings = GetCommandSettings(testContext, filename);
				return generatorService.Generate(commandSettings);
			}
		}

		protected CommandSettings GetCommandSettings(TestContext testContext, string filename)
		{
			var path = testContext.GetTestInputFullPath(filename);
			return new CommandSettings
			{
				ConfigPath = path,
				RootPath = RootPath
			};
		}
	}
}