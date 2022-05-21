using System.Collections.Generic;
using Gunslinger.Types;
using Microsoft.SqlServer.Management.Smo;
using Models;
using Models.SQL;

namespace Gunslinger.Factories.SQL
{
    public class SQLPropertyFactory
    {
        private static List<string> lengthTypes = new List<string> {
            "nvarchar",
            "varchar",
            "binary",
            "char",
            "datetime2",
            "datetimeoffset",
            "decimal",
            "nchar",
            "decimal",
            "numeric",
            "time",
            "varbinary"
        };

        private static List<string> maxTypes = new List<string> {
            "varcharmax",
            "nvarcharmax",
            "varbinarymax"
        };

        public static SQLColumn Create(Name modelName, Column column, Models.Enums.Language language, Template template)
        {
            var property = new SQLColumn
            {
                DefaultValue = column.DefaultConstraint?.Text,
                Length = column.DataType.MaximumLength,
                Name = NameFactory.Create(column.Name, template, false),
                Nullable = column.Nullable,
                PrimaryKey = column.InPrimaryKey,
                SqlDataTypeEnum = column.DataType.SqlDataType,
                SqlDataType = getDataType(Models.Enums.Language.sql, column.DataType.SqlDataType, column.DataType.MaximumLength),
                Type = getDataType(language, column.DataType.SqlDataType, 0),
                IsInPrimaryKey = column.InPrimaryKey,
                IsForeignKey = column.IsForeignKey,
                ModelName = modelName
            };
            return property;
        }

        private static string getDataType(Models.Enums.Language language, SqlDataType sqlDataType, int maximumLength)
        {
            switch (language)
            {
                case Models.Enums.Language.csharp:
                    return SQLDataTypeConversion.ConvertTo_CSDataType(sqlDataType, false);

                case Models.Enums.Language.sql:
                    var baseType = sqlDataType.ToString().ToLower();
                    if (maxTypes.Contains(baseType))
                    {
                        return $"{baseType.Replace("max", "")}(max)";
                    }
                    if (lengthTypes.Contains(baseType))
                    {
                        return $"{baseType}({maximumLength})";
                    }
                    return baseType;

                default:
                    return "string";
            }
        }
    }
}