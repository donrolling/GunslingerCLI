using Domain.Models.General;
using Domain.Models.SQL;
using Microsoft.SqlServer.Management.Smo;

namespace Gunslinger.Factories.SQL
{
	public class ForeignKeyFactory
	{
		public static SQLForeignKey Create(
			string sourceTableName,
			string sourceSchemaName,
			string sourceColumnName,
			bool sourceColumnNullable,
			string referenceTableName,
			string referenceSchemaName,
			string referenceColumnName,
			bool referenceColumnNullable,
			SqlDataType sqlDataType,
			Template template
		)
		{
			var sourceColumnSource = ColumnSourceFactory.Create(
				sourceTableName,
				sourceSchemaName,
				sourceColumnName,
				sqlDataType,
				sourceColumnNullable,
				template
			);
			var referenceColumnSource = ColumnSourceFactory.Create(
				referenceTableName,
				referenceSchemaName,
				referenceColumnName,
				sqlDataType,
				referenceColumnNullable,
				template
			);

			return new SQLForeignKey
			{
				IsNullable = sourceColumnNullable || referenceColumnNullable, // todo: sharing this value is sloppy, but ok for now
				Reference = sourceColumnSource,
				Source = referenceColumnSource
			};
		}

		public static List<SQLForeignKey> Create(IEnumerable<Table> tables, Template template)
		{
			var sqlForeignKeys = new List<SQLForeignKey>();
			foreach (var table in tables)
			{
				foreach (ForeignKey key in table.ForeignKeys)
				{
					var fkColumn = key.Columns[0];
					var sqlDataType = SqlDataType.BigInt; // just assigning a default value so the compiler doesn't get mad
														  // sharing this value between both tables because looking up the FK Table and looping through its
														  // columns doesn't seem worth it. Propbably should do it though.
					var isNullable = false; // todo: sloppy, fix
											// find the column by looping (lame, but have to)
											// set properties once you find it
					foreach (Column column in table.Columns)
					{
						if (column.Name == fkColumn.Name)
						{
							sqlDataType = column.DataType.SqlDataType;
							isNullable = column.Nullable;
							break;
						}
					}
					var fk = ForeignKeyFactory.Create(
						table.Name,
						table.Schema,
						fkColumn.Name,
						isNullable,
						key.ReferencedTable,
						key.ReferencedTableSchema,
						fkColumn.ReferencedColumn,
						isNullable,
						sqlDataType,
						template
					);
					sqlForeignKeys.Add(fk);
				}
			}
			return sqlForeignKeys;
		}
	}
}