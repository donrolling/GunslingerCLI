//using Contracts;
//using Microsoft.Extensions.Logging;
//using Models;
//using Models.BaseClasses;

//namespace Gunslinger.Factories
//{
//	public class GenerationContextFactory : LoggingWorker, IContextFactory
//	{
//		private readonly IFileProvider _fileProvider;
//		private static GenerationContext _generationContext;

//		public GenerationContextFactory(
//			IFileProvider fileProvider,
//			GenerationContext generationContext,
//			ILoggerFactory loggerFactory
//		) : base(loggerFactory)
//		{
//			_fileProvider = fileProvider;
//			_generationContext = generationContext;
//		}

//		public GenerationContext Create(CommandSettings commandSettings)
//		{
//			var path = Directory.GetCurrentDirectory();
//			//if the user set it to a full path, let them use it, otherwise it is a relative path
//			if (!_generationContext.TemplateDirectory.Contains(":\\"))
//			{
//				_generationContext.TemplateDirectory = $"{path}\\{_generationContext.TemplateDirectory}";
//			}
//			foreach (var template in _generationContext.Templates)
//			{
//				var templatePath = $"{_generationContext.TemplateDirectory}\\{template.InputRelativePath}";
//				var text = _fileProvider.Get(templatePath);
//				if (string.IsNullOrEmpty(text))
//				{
//					throw new Exception($"Template was empty {template.Name}");
//				}
//				template.TemplateText = text;
//			}

//			return _generationContext;
//		}
//	}
//}