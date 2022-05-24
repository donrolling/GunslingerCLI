using Domain.Enums;
using Models;

namespace Domain.Models.General
{
	public class Template
	{
		public List<string> ExcludeTheseTypes { get; set; } = new List<string>();
		public string DataProviderName { get; set; }
		public bool DeleteAllItemsInOutputDirectory { get; set; } = true;
		public List<string> Imports { get; set; } = new List<string>();
		public bool IsStub { get; set; }
		public ProgrammingLanguage Language { get; set; }
		public string Name { get; set; }
		public string Namespace { get; set; }
		public string InputRelativePath { get; set; }
		public string OutputRelativePath { get; set; }
		public string TemplateText { get; set; }
		public TemplateType Type { get; set; }
		public PluralizationSettings PluralizationSettings { get; set; } = PluralizationSettings.None;
		public List<FileRename> FileRename { get; set; }
	}
}