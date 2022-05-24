using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models.General;
using Domain.Models.SQL;
using Microsoft.SqlServer.Management.Smo;

using Omu.ValueInjecter;

namespace Gunslinger.Factories.SQL
{
	public class SQLTableFactory
    {
        public static IEnumerable<SQLTable> Create(string @namespace, Models.Enums.Language language, IEnumerable<Table> tables, Template template)
        {
            // get all the foreign key meta data first
            var sqlForeignKeys = ForeignKeyFactory.Create(tables, template);
            var sqlTables = new List<SQLTable>();
            foreach (var table in tables)
            {
                var sqlBasicTableCreateResult = createBasicTable(@namespace, language, table, template);
                if (sqlBasicTableCreateResult.Failed)
                {
                    continue;
                }
                var sqlTable = create(sqlBasicTableCreateResult.Result, sqlForeignKeys);
                sqlTables.Add(sqlTable);
            }
            return sqlTables;
        }

        private static SQLTable create(SQLBasicTable sqlBasicTable, List<SQLForeignKey> allSQLForeignKeys)
        {
            var sqlTable = new SQLTable();
            sqlTable.InjectFrom(sqlBasicTable);
            sqlTable.ForeignKeys = allSQLForeignKeys.Where(a => a.Reference.UniqueName == sqlBasicTable.UniqueName).ToList();
            return sqlTable;
        }

        private static OperationResult<SQLBasicTable> createBasicTable(string @namespace, Models.Enums.Language language, Table table, Template template)
        {
            if (string.IsNullOrEmpty(table.Name))
            {
                throw new Exception("Table names must not be empty.");
            }
            var modelName = NameFactory.Create(table.Name, template, true);
            var sqlColumns = getSQLColumns(modelName, language, table.Columns, template);
            var sqlKeys = getSQLKeys(sqlColumns, template);
            if (!sqlKeys.Any())
            {
                return OperationResult.Fail<SQLBasicTable>("No Keys.");
            }
            var uniqueName = UniqueNameFactory.Create(table.Schema, table.Name);
            // kinda assuming there is only one key for now
            var entity = new SQLBasicTable
            {
                TableName = NameFactory.Create(table.Name, template, false), // tablename does not represent a class 
                UniqueName = uniqueName,
                Name = NameFactory.Create(table.Name, template, true), // name does represent a class 
                Schema = table.Schema,
                Key = sqlKeys.FirstOrDefault(),
                Keys = sqlKeys,
                Columns = sqlColumns,
                Namespace = @namespace
            };
            return OperationResult.Ok(entity);
        }

        private static List<SQLKey> getSQLKeys(List<SQLColumn> sqlColumns, Template template)
        {
            var result = new List<SQLKey>();
            foreach (var sqlColumn in sqlColumns.Where(a => a.PrimaryKey))
            {
                var key = KeyFactory.Create(sqlColumn, template);
                result.Add(key);
            }
            return result;
        }

        private static List<SQLColumn> getSQLColumns(Name modelName, Models.Enums.Language language, ColumnCollection columns, Template template)
        {
            var result = new List<SQLColumn>();
            foreach (Column column in columns)
            {
                var property = SQLPropertyFactory.Create(modelName, column, language, template);
                result.Add(property);
            }
            return result;
        }
    }
}