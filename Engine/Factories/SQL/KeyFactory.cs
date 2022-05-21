using Gunslinger.Types;
using Microsoft.SqlServer.Management.Smo;
using Models;
using Models.SQL;

namespace Gunslinger.Factories.SQL
{
    public class KeyFactory
    {
        public static SQLKey Create(SQLColumn sqlColumn, Template template)
        {
            var name = NameFactory.Create(sqlColumn.Name.Value, template, false);
            var dataType = SQLDataTypeConversion.ConvertTo_CSDataType(sqlColumn.SqlDataTypeEnum, false);
            var dbType = SQLDataTypeConversion.ConvertTo_CSDbType(sqlColumn.SqlDataTypeEnum);
            return new SQLKey
            {
                Name = name,
                IsNullable = sqlColumn.Nullable,
                SQLDataType = sqlColumn.SqlDataTypeEnum.ToString(),
                DataType = dataType,
                DbType = dbType
            };
        }

        public static SQLKey Create(string columnName, SqlDataType sqlDataType, bool nullable, Template template)
        {
            var name = NameFactory.Create(columnName, template, false);
            var dataType = SQLDataTypeConversion.ConvertTo_CSDataType(sqlDataType, false);
            var dbType = SQLDataTypeConversion.ConvertTo_CSDbType(sqlDataType);
            return new SQLKey
            {
                Name = name,
                IsNullable = nullable,
                SQLDataType = sqlDataType.ToString(),
                DataType = dataType,
                DbType = dbType
            };
        }
    }
}