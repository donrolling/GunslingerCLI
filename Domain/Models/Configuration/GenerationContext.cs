﻿using Contracts;
using Domain.Models.General;

namespace Domain.Models.Configuration
{
	public class GenerationContext
	{
		public List<string> AuditProperties { get; set; } = new List<string>();
		public string ConfigPath { get; set; }
		public List<IDataProviderSettings> DataProviders { get; set; } = new List<IDataProviderSettings>();
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