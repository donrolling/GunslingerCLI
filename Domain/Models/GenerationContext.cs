﻿using Contracts;
using Models;

namespace Domain.Models
{
	public class GenerationContext
	{
		public List<string> AuditProperties { get; set; } = new List<string>();
		public List<IDataProvider> DataProviders { get; set; } = new List<IDataProvider>();
		public List<string> ExcludeTheseEntities { get; set; } = new List<string>();
		public List<string> ExcludeTheseTemplates { get; set; } = new List<string>();
		public List<string> IncludeTheseEntitiesOnly { get; set; } = new List<string>();
		public List<string> IncludeTheseTemplatesOnly { get; set; } = new List<string>();
		public string OutputDirectory { get; set; }
		public bool ProcessTemplateStubs { get; set; } = true;
		public List<Resource> Resources { get; set; }
		public string RootPath { get; set; }
		public string TemplateDirectory { get; set; }
		public List<Template> Templates { get; set; } = new List<Template>();
	}
}