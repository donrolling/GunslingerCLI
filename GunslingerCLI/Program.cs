using CommandLine;
using Contracts;
using GunslingerCLI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Models.Enums;

var commandSettingsResult = GetOptions(args);
var host = Bootstrapper.Configuration.ConfigureServices();
using (var scope = host.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var logger = services.GetRequiredService<ILogger<Program>>();
	var generatorService = services.GetRequiredService<IGeneratorService>();

	if (commandSettingsResult.Failed)
	{
		logger.LogError(commandSettingsResult.Message);
		return;
	}

	var result = generatorService.Generate(commandSettingsResult.Result);
	if (result.Success)
	{
		logger.LogInformation("Model generation succeeded.");
	}
	else
	{
		logger.LogError("Model generation failed.");
	}
	logger.LogInformation(result.Message);
	// this is required because serilog doesn't flush messages quite fast enough if you don't do this.
	Thread.Sleep(TimeSpan.FromSeconds(1));
}

static OperationResult<CommandSettings> GetOptions(string[] args)
{
	var options = Parser.Default.ParseArguments<Options>(args);
	if (options.Errors.Any())
	{
		var message = string.Join(Environment.NewLine, options.Errors);
		return OperationResult.Fail<CommandSettings>(message, Status.Cancelled);
	}
	var commandLineArgs = Environment.GetCommandLineArgs();
	if (commandLineArgs == null)
	{
		return OperationResult.Fail<CommandSettings>("Command Line Arguments are missing.", Status.Cancelled);
	}
	var terminalRoot = commandLineArgs[0];
	if (string.IsNullOrWhiteSpace(terminalRoot))
	{
		return OperationResult.Fail<CommandSettings>("Command Line Arguments are missing.", Status.Cancelled);
	}
	// test for "running in console" situation
	// hopefully this situation won't ever be what happens when running this as a tool
	// in that case, here is an example of the first argument: R:\\GitHub\\Gunslinger\\GunslingerCLI\\GunslingerCLI\\bin\\Debug\\net6.0\\GunslingerCLI.dll
	// we'll pull the filename off and use that path for the root path
	if (File.Exists(terminalRoot))
	{
		var directoryInfo = new DirectoryInfo(terminalRoot);
		terminalRoot = directoryInfo.Parent.FullName;
	}
	var commandSettings = new CommandSettings
	{
		ConfigPath = options.Value.ConfigPath,
		RootPath = terminalRoot
	};
	return OperationResult.Ok(commandSettings);
}