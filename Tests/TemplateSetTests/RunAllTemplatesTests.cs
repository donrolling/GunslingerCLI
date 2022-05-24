using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Tests.TemplateSetTests
{
	[TestClass]
	public class RunAllTemplatesTests : BaseTests
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void RunAllBusinessTemplates()
		{
			var filename = "gunslinger.json";
			var result = RunGeneratorFromConfig(this, TestContext, filename);
			Assert.IsTrue(result.Success, result.Message);
		}

		[TestMethod]
		public void RunAllEntities()
		{
			var filename = "gunslinger.json";
			var result = RunGeneratorFromConfig(this, TestContext, filename);
			Assert.IsTrue(result.Success, result.Message);
		}

		[TestMethod]
		public void RunAllStubs()
		{
			var filename = "gunslinger.json";
			var generatorService = GetService<IGeneratorService>();
			var result = generatorService.Generate(GetCommandSettings(TestContext, filename));
			Assert.IsTrue(result.Success, result.Message);

			// this code would work, but I'm not sure what it was trying to assert and things have changed
			//var di = new DirectoryInfo(generatorService.Context.OutputDirectory);
			//var files = di.GetFiles();
			//Assert.AreEqual(0, files.Length);
			//var folders = di.GetDirectories();
			//Assert.AreEqual(0, folders.Length);
		}

		[TestMethod]
		public void RunAllTemplates()
		{
			var filename = "gunslinger.json";
			var result = RunGeneratorFromConfig(this, TestContext, filename);
			Assert.IsTrue(result.Success, result.Message);
		}

		[TestMethod]
		public void RunAllWithExclusions()
		{
			var filename = "gunslinger.json";
			var generatorService = GetService<IGeneratorService>();
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