using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.TemplateSetTests
{
	[TestClass]
	public class StoredProcInquiryTests : BaseTests
	{
		public TestContext TestContext { get; set; }

		public StoredProcInquiryTests()
		{

		}

		[TestMethod]
		public void Run()
		{
			var filename = "gunslinger.json";
			var result = RunGeneratorFromConfig(this, TestContext, filename);
			Assert.IsTrue(result.Success, result.Message);
		}
	}
}