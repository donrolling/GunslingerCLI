using Domain.Enums;
using Domain.Models.General;
using Pluralize.NET.Core;
using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Engine.Factories
{
	public class NameFactory
	{
		public static Regex rgx = new Regex("[^a-zA-Z0-9 -]");

		public static Name Create(string name)
		{
			return MakeName(name);
		}

		public static Name Create(string name, Template template, bool isClass)
		{
			// don't use pluralization on anything but class names
			if (!isClass)
			{
				return MakeName(name);
			}
			var pluralizer = new Pluralizer();
			switch (template.PluralizationSettings)
			{
				case PluralizationSettings.Plural:
					var pluralName = pluralizer.Pluralize(name);
					return MakeName(pluralName);

				case PluralizationSettings.Singular:
					var singularName = pluralizer.Singularize(name);
					return MakeName(singularName);

				case PluralizationSettings.None:
				default:
					return MakeName(name);
			}
		}

		private static Name MakeName(string name)
		{
			return new Name
			{
				Value = name,
				LowerCamelCase = rgx.Replace(name.ToCamelCase(), ""),
				PascalCase = rgx.Replace(name.ToPascalCase(), ""),
				NameWithSpaces = name.UnCamelCase(),
			};
		}
	}
}