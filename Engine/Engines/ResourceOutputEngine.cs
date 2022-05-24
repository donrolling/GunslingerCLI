using Contracts;
using Domain.Models;
using Domain.Models.General;
using Microsoft.Extensions.Logging;

namespace Gunslinger.Engines
{
	public class ResourceOutputEngine : IResourceOutputEngine
	{
		private readonly ITemplateOutputEngine _templateOutputEngine;
		private readonly ILogger _logger;

		public ResourceOutputEngine(ITemplateOutputEngine templateOutputEngine, ILoggerFactory loggerFactory)
		{
			this._templateOutputEngine = templateOutputEngine;
			this._logger = loggerFactory.CreateLogger(this.GetType().Name);
		}

		public OperationResult Write(GenerationContext settings)
		{
			var currentDirectory = Directory.GetCurrentDirectory();
			foreach (var resource in settings.Resources)
			{
				var source = resource.Source.Contains(":\\")
					? resource.Source
					: $"{currentDirectory}\\{resource.Source}";
				var destination = $"{settings.OutputDirectory}\\{resource.Destination}";
				var result = copy(source, destination);
				if (result.Failed)
				{
					return result;
				}
			}
			return OperationResult.Ok();
		}

		private OperationResult copy(string sourcePath, string destinationPath)
		{
			// is file or directory
			var isDirectory = Directory.Exists(sourcePath);
			var isFile = File.Exists(sourcePath);
			if (!isFile && !isDirectory)
			{
				return OperationResult.Fail($"Invalid source path: {sourcePath}");
			}

			try
			{
				if (isDirectory)
				{
					return createFilesAndDirectories(sourcePath, destinationPath);
				}

				if (isFile)
				{
					// Copy all the files & Replaces any files with the same name
					File.Copy(sourcePath, destinationPath, true);
				}
			}
			catch (System.Exception ex)
			{
				return OperationResult.Fail(ex.Message);
			}

			return OperationResult.Ok();
		}

		private OperationResult createFilesAndDirectories(string sourcePath, string destinationPath)
		{
			// cleanup the output directory
			var cleanupResult = _templateOutputEngine.CleanupOutputDirectory(destinationPath);
			copyAllDirectories(sourcePath, destinationPath);
			copyAllFiles(sourcePath, destinationPath);
			return OperationResult.Ok();
		}

		private static void copyAllDirectories(string sourcePath, string destinationPath)
		{
			//create all of the directories
			var sourcePaths = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
			foreach (var newSourcePath in sourcePaths)
			{
				var newDestinationPath = newSourcePath.Replace(sourcePath, destinationPath);
				Directory.CreateDirectory(newDestinationPath);
			}
		}

		private static void copyAllFiles(string sourcePath, string destinationPath)
		{
			//Copy all the files & Replaces any files with the same name
			var sourcePaths = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
			foreach (var newSourcePath in sourcePaths)
			{
				var newDestinationPath = newSourcePath.Replace(sourcePath, destinationPath);
				File.Copy(newSourcePath, newDestinationPath, true);
			}
		}
	}
}