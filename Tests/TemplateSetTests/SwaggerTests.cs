using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Tests.TemplateSetTests
{
    [TestClass]
    public class SwaggerTests : BaseTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void RunAllTemplates()
        {
            var filename = "SwaggerConfig.json";
            var result = RunGeneratorFromConfig(this, TestContext, filename);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RunAllTemplates_RenameOneFile()
        {
            var filename = "SwaggerConfig.json";
            var generatorService = GetGeneratorService();
            var result = generatorService.Generate(GetCommandSettings(TestContext, filename));
            var template = generatorService.Context.Templates.First();
            var badFilePath = Path.Join(generatorService.Context.OutputDirectory, template.OutputRelativePath.Replace("{entityName}", "Item"));
            var goodFilePath = Path.Join(generatorService.Context.OutputDirectory, template.OutputRelativePath.Replace("{entityName}", "PolicyEntityGraph"));
            var badFile = new FileInfo(badFilePath);
            var goodFile = new FileInfo(goodFilePath);
            Assert.IsTrue(result.Success, $"This process should have succeeded. Error: {result.Message}");
            Assert.IsFalse(badFile.Exists, $"{badFilePath} shouldn't exist.");
            Assert.IsTrue(goodFile.Exists, $"{goodFilePath} should exist.");
            var contents = File.ReadAllText(goodFilePath);
            Assert.IsFalse(contents.Contains("public class Item"), "File should have had its contents replaced to reflect the new name.");
            Assert.IsTrue(contents.Contains("public class PolicyEntityGraph"), "File should have had its contents replaced to reflect the new name.");
        }

        [TestMethod]
        public void RunAllWithLocalDataSource()
        {
            var filename = "SwaggerConfigLocalDataSource.json";
            var result = RunGeneratorFromConfig(this, TestContext, filename);
            Assert.IsTrue(result.Success);
        }
    }
}