using Contracts;
using Domain.Models.General;
using Microsoft.Extensions.Logging;
using Models.BaseClasses;
using Stubble.Core;
using Stubble.Core.Builders;

namespace Gunslinger.Engines
{
	public class StubbleRenderEngine : LoggingWorker, IRenderEngine
    {
        private static readonly StubbleVisitorRenderer _stubbleVisitorRenderer = new StubbleBuilder().Build();

        public StubbleRenderEngine(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public string Render(Template template, IProviderModel model)
        {
            model.Imports = template.Imports;
            return _stubbleVisitorRenderer.Render(template.TemplateText, model);
        }

        public string Render(Template template, IGroupProviderModel model)
        {
            model.Imports = template.Imports;
            return _stubbleVisitorRenderer.Render(template.TemplateText, model);
        }
    }
}