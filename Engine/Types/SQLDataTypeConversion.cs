using Microsoft.SqlServer.Management.Smo;
using System.Data;

namespace Gunslinger.Types
{
    public static class SQLDataTypeConversion
    {
        /// <summary>
        /// Conversion from SQLDataType to CSharp DataType.
        /// I think I got all of these right
        /// Here is the chart: http://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static string ConvertTo_CSDataType(SqlDataType dataType, bool isNullable)
        {
            var csDataType = string.Empty;
            switch (dataType)
            {
                case SqlDataType.BigInt:
                    csDataType = "long";
                    break;

                case SqlDataType.Binary:
                case SqlDataType.VarBinary:
                case SqlDataType.VarBinaryMax:
                    csDataType = "byte[]";
                    break;

                case SqlDataType.Int:
                case SqlDataType.SmallInt:
                    csDataType = "int";
                    break;

                case SqlDataType.TinyInt:
                    csDataType = "byte";
                    break;

                case SqlDataType.Real:
                    csDataType = "Single";
                    break;

                case SqlDataType.Bit:
                    csDataType = "bool";
                    break;

                case SqlDataType.Variant:
                    csDataType = "object";
                    break;

                case SqlDataType.SmallMoney:
                case SqlDataType.Decimal:
                case SqlDataType.Money:
                case SqlDataType.Numeric:
                    csDataType = "decimal";
                    break;

                case SqlDataType.Float:
                    csDataType = "double";
                    break;

                case SqlDataType.Time:
                    csDataType = "TimeSpan";
                    break;

                case SqlDataType.Timestamp:
                    csDataType = "byte[]";
                    break;

                case SqlDataType.DateTime:
                case SqlDataType.DateTime2:
                    csDataType = "DateTime";
                    break;

                case SqlDataType.DateTimeOffset:
                    csDataType = "DateTimeOffset";
                    break;

                case SqlDataType.UniqueIdentifier:
                    csDataType = "Guid";
                    break;

                case SqlDataType.Xml:
                    csDataType = "Xml";
                    break;

                case SqlDataType.Text:
                case SqlDataType.Char:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.VarChar:
                case SqlDataType.NVarCharMax:
                case SqlDataType.VarCharMax:
                default:
                    csDataType = "string";
                    break;
            }

            var result = isNullable ? adjustDataTypeToNullable(dataType, csDataType) : csDataType;
            return result;
        }

        private static string adjustDataTypeToNullable(SqlDataType dataType, string csDataType)
        {
            var result = string.Empty;
            switch (dataType)
            {
                case SqlDataType.BigInt:
                case SqlDataType.Binary:
                case SqlDataType.VarBinary:
                case SqlDataType.Int:
                case SqlDataType.SmallInt:
                case SqlDataType.TinyInt:
                case SqlDataType.Real:
                case SqlDataType.Bit:
                case SqlDataType.Variant:
                case SqlDataType.SmallMoney:
                case SqlDataType.Decimal:
                case SqlDataType.Money:
                case SqlDataType.Numeric:
                case SqlDataType.Float:
                case SqlDataType.Time:
                case SqlDataType.DateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                case SqlDataType.UniqueIdentifier:
                case SqlDataType.Xml:
                    result = "?";
                    break;

                case SqlDataType.Timestamp:
                case SqlDataType.Text:
                case SqlDataType.Char:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.VarChar:
                case SqlDataType.NVarCharMax:
                case SqlDataType.VarBinaryMax:
                case SqlDataType.VarCharMax:
                default:
                    result = string.Empty;
                    break;
            }

            return string.Concat(csDataType, result);
        }

        public static DataType Convert_CSDataType_to_SqlDataType(string typeName)
        {
            var type = SqlDataType.NVarChar;
            switch (typeName)
            {
                case "long":
                    type = SqlDataType.BigInt;
                    break;

                case "byte[]":
                    type = SqlDataType.Binary;
                    break;

                case "int":
                    type = SqlDataType.Int;
                    break;

                case "byte":
                    type = SqlDataType.TinyInt;
                    break;

                case "Single":
                    type = SqlDataType.Real;
                    break;

                case "bool":
                    type = SqlDataType.Bit;
                    break;

                case "object":
                    type = SqlDataType.Variant;
                    break;

                case "Decimal":
                    type = SqlDataType.Decimal;
                    break;

                case "Double":
                    type = SqlDataType.Float;
                    break;

                case "TimeSpan":
                    type = SqlDataType.Time;
                    break;

                case "DateTime":
                    type = SqlDataType.DateTime;
                    break;

                case "DateTimeOffset":
                    type = SqlDataType.DateTimeOffset;
                    break;

                case "Guid":
                    type = SqlDataType.UniqueIdentifier;
                    break;

                case "Xml":
                    type = SqlDataType.Xml;
                    break;

                case "string":
                default:
                    type = SqlDataType.NVarChar;
                    break;
            }
            return new DataType(type);
        }

        public static string GetDataTypeDeclaration(DataType dataType)
        {
            string result = dataType.Name;
            switch (dataType.SqlDataType)
            {
                case SqlDataType.Binary:
                case SqlDataType.Char:
                case SqlDataType.NChar:
                case SqlDataType.NVarChar:
                case SqlDataType.VarBinary:
                case SqlDataType.VarChar:
                    result += string.Format("({0})", dataType.MaximumLength);
                    break;

                case SqlDataType.NVarCharMax:
                case SqlDataType.VarBinaryMax:
                case SqlDataType.VarCharMax:
                    result += "(max)";
                    break;

                case SqlDataType.Decimal:
                case SqlDataType.Numeric:
                    result += string.Format("({0}, {1})", dataType.NumericPrecision, dataType.NumericScale);
                    break;
            }
            return result;
        }

        public static string ConvertTo_CSDbType(SqlDataType sqlDataType)
        {
            switch (sqlDataType)
            {
                case SqlDataType.None:
                    return string.Empty;

                case SqlDataType.BigInt:
                    return DbType.Int64.ToString();

                case SqlDataType.Binary:
                    return DbType.Binary.ToString();

                case SqlDataType.Bit:
                    return DbType.Boolean.ToString();

                case SqlDataType.Char:
                    return DbType.String.ToString();

                case SqlDataType.DateTime:
                    return DbType.DateTime.ToString();

                case SqlDataType.Decimal:
                    return DbType.Decimal.ToString();

                case SqlDataType.Float:
                    return DbType.Double.ToString();

                case SqlDataType.Image:
                    return string.Empty;

                case SqlDataType.Int:
                    return DbType.Int32.ToString();

                case SqlDataType.Money:
                    return DbType.Currency.ToString();

                case SqlDataType.NChar:
                    return DbType.String.ToString();

                case SqlDataType.NText:
                    return DbType.String.ToString();

                case SqlDataType.NVarChar:
                    return DbType.String.ToString();

                case SqlDataType.NVarCharMax:
                    return DbType.String.ToString();

                case SqlDataType.Real:
                    return DbType.Double.ToString();

                case SqlDataType.SmallDateTime:
                    return DbType.DateTime.ToString();

                case SqlDataType.SmallInt:
                    return DbType.Int16.ToString();

                case SqlDataType.SmallMoney:
                    return DbType.Currency.ToString();

                case SqlDataType.Text:
                    return DbType.String.ToString();

                case SqlDataType.Timestamp:
                    return DbType.Time.ToString();

                case SqlDataType.TinyInt:
                    return DbType.Int16.ToString();

                case SqlDataType.UniqueIdentifier:
                    return DbType.Guid.ToString();

                case SqlDataType.UserDefinedDataType:
                    return string.Empty;

                case SqlDataType.UserDefinedType:
                    return string.Empty;

                case SqlDataType.VarBinary:
                    return DbType.Binary.ToString();

                case SqlDataType.VarBinaryMax:
                    return DbType.Binary.ToString();

                case SqlDataType.VarChar:
                    return DbType.String.ToString();

                case SqlDataType.VarCharMax:
                    return DbType.String.ToString();

                case SqlDataType.Variant:
                    return DbType.String.ToString();

                case SqlDataType.Xml:
                    return DbType.Xml.ToString();

                case SqlDataType.SysName:
                    return DbType.String.ToString();

                case SqlDataType.Numeric:
                    return DbType.VarNumeric.ToString();

                case SqlDataType.Date:
                    return DbType.Date.ToString();

                case SqlDataType.Time:
                    return DbType.Time.ToString();

                case SqlDataType.DateTimeOffset:
                    return DbType.DateTimeOffset.ToString();

                case SqlDataType.DateTime2:
                    return DbType.DateTime2.ToString();

                case SqlDataType.UserDefinedTableType:
                    return string.Empty;

                case SqlDataType.HierarchyId:
                    return string.Empty;

                case SqlDataType.Geometry:
                    return string.Empty;

                case SqlDataType.Geography:
                    return string.Empty;

                default:
                    return string.Empty;
            }
        }
    }
}