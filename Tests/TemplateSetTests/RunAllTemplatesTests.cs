using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using Tests.TestExtensions;
using Tests.Utilities;

namespace Tests.TemplateSetTests
{
	[TestClass]
	public class RunAllTemplatesTests : BaseTests
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void RunAllStubbs()
		{
			var filename = "GenerationContextAllStubs.json";
			var generatorService = GetGeneratorService();
			var result = generatorService.Generate(GetCommandSettings(TestContext, filename));
			Assert.IsTrue(result.Success, result.Message);

			var di = new DirectoryInfo(generatorService.Context.OutputDirectory);
			var files = di.GetFiles();
			Assert.AreEqual(0, files.Length);
			var folders = di.GetDirectories();
			Assert.AreEqual(0, folders.Length);
		}

		[TestMethod]
		public void RunAllBusinessTemplates()
		{
			var filename = "GenerationContext.Business.json";
			var result = RunGeneratorFromConfig(this, TestContext, filename);
			Assert.IsTrue(result.Success, result.Message);
		}

		[TestMethod]
		public void RunAllEntities()
		{
			var filename = "GenerationContext.Entities.json";
			var result = RunGeneratorFromConfig(this, TestContext, filename);
			Assert.IsTrue(result.Success, result.Message);
		}

		[TestMethod]
		public void RunAllTemplates()
		{
			var filename = "GenerationContext.json";
			var result = RunGeneratorFromConfig(this, TestContext, filename);
			Assert.IsTrue(result.Success, result.Message);
		}

		[TestMethod]
		public void RunAllWithExclusions()
		{
			var filename = "GenerationContextExclusions.json";
			var generatorService = GetGeneratorService();
			var result = generatorService.Generate(GetCommandSettings(TestContext, filename));
			Assert.IsTrue(result.Success, result.Message);

			var di = new DirectoryInfo(generatorService.Context.OutputDirectory);
			var templates = generatorService.Context.Templates.First();
			Assert.IsNotNull(templates);
			Assert.IsTrue(templates.ExcludeTheseTypes.Any());
			var files = di.GetFiles();
			var any = files.Any(a => templates.ExcludeTheseTypes.Contains(a.Name.Replace(".cs", "")));
			Assert.IsFalse(any);
		}

	}
}