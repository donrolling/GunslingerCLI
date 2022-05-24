using Domain.Models.SQL;
using Microsoft.SqlServer.Management.Smo;
using Models;
using Omu.ValueInjecter;
using Pluralize.NET.Core;

namespace Gunslinger.Factories.SQL
{
	public class ColumnSourceFactory
	{
		public static ColumnSource Create(string tableName, string schemaName, string columnName, SqlDataType sqlDataType, bool nullable, Template template)
		{
			var key = KeyFactory.Create(columnName, sqlDataType, nullable, template);

			var columnSource = new ColumnSource
			{
				UniqueName = UniqueNameFactory.Create(schemaName, tableName),
				TablePlural = new Pluralizer().Pluralize(tableName),
				Table = NameFactory.Create(tableName, template, true),
				Schema = schemaName
			};

			columnSource.InjectFrom(key);

			return columnSource;
		}
	}
}