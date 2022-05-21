using Contracts;
using Models;
using Models.BaseClasses;
using HandlebarsDotNet;
using Microsoft.Extensions.Logging;

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
                if (type0 == "UndefinedBindingResult")
                {
                    throw new Exception("Argument 0 undefined");
                }
                if (type1 == "UndefinedBindingResult")
                {
                    throw new Exception("Argument 1 undefined");
                }
                if (type0 == "String" && type1 == "String")
                {
                    if (arguments[0].Equals(arguments[1]))
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
                    if (arguments[0] == arguments[1])
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
                if ((bool)arguments[0])
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