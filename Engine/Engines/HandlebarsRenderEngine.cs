using Contracts;
using Models.BaseClasses;
using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using Domain.Models.General;

namespace Gunslinger.Engines
{
	public class HandlebarsRenderEngine : LoggingWorker, IRenderEngine
    {
        public HandlebarsRenderEngine(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            // register an ifCond helper so I can write if statements in the templates
            Handlebars.RegisterHelper("ifCond", (writer, options, context, arguments) =>
            {
                var type0 = arguments[0].GetType().Name;
                var type1 = arguments[1].GetType().Name;
                var argument0 = arguments[0];
                var argument1 = arguments[1];
                if (type0 == "UndefinedBindingResult")
                {
                    var message = $"argument0: {argument0} - argument1: {argument1}";
                    throw new Exception($"Argument 0 undefined. Detail: {message}");
                }
                if (type1 == "UndefinedBindingResult")
                {
                    var message = $"argument0: {argument0} - argument1: {argument1}";
                    throw new Exception($"Argument 1 undefined. Detail: {message}");
                }
                if (type0 == "string" && type1 == "string")
                {
                    if (argument0.Equals(argument1))
                    {
                        options.Template(writer, (object)context);
                    }
                    else
                    {
                        options.Inverse(writer, (object)context);
                    }
                }
                else
                {
                    if (argument0 == argument1)
                    {
                        options.Template(writer, (object)context);
                    }
                    else
                    {
                        options.Inverse(writer, (object)context);
                    }
                }
            });
            // register an ifCond helper so I can write if statements in the templates
            Handlebars.RegisterHelper("boolCond", (writer, options, context, arguments) =>
            {
                var arguments0 = arguments[0];
                if ((bool)arguments0)
                {
                    options.Template(writer, (object)context);
                }
                else
                {
                    options.Inverse(writer, (object)context);
                }
            });
        }

        public string Render(Template template, IProviderModel model)
        {
            model.Imports = template.Imports;
            var handlebarsTemplate = Handlebars.Compile(template.TemplateText);
            var result = handlebarsTemplate(model);
            return result;
        }

        public string Render(Template template, IGroupProviderModel model)
        {
            model.Imports = template.Imports;
            var handlebarsTemplate = Handlebars.Compile(template.TemplateText);
            var result = handlebarsTemplate(model);
            return result;
        }
    }
}