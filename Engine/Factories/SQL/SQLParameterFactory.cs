using Domain.Enums;
using Domain.Models.General;
using Domain.Models.SQL;
using Microsoft.SqlServer.Management.Smo;

namespace Engine.Factories.SQL
{
	public class SQLParameterFactory
	{
		public static SQLParameter Create(Name modelName, Column column, ProgrammingLanguage language, Template template)
		{
			var param = new SQLParameter
			{
				DefaultValue = column.DefaultConstraint?.Text,
				Length = column.DataType.MaximumLength,
				Name = NameFactory.Create(column.Name, template, false),
				IsNullable = column.Nullable,
				SqlDataType = getDataType(ProgrammingLanguage.sql, column.DataType.SqlDataType, column.DataType.MaximumLength),
				Type = getDataType(language, column.DataType.SqlDataType, 0),
			};
			return param;
		}
	}
}