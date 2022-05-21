using System;
using Microsoft.SqlServer.Management.Smo;

namespace Models.SQL
{
    public class SQLColumn : Property
    {
        public bool IsInPrimaryKey { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool PrimaryKey { get; set; }
        public SqlDataType SqlDataTypeEnum { get; set; }
        public string SqlDataType { get; set; }
        public bool Nullable { get; set; }
        public object DefaultValue { get; set; }
        public int Length { get; set; }
        public bool IsForeignKey { get; set; }
    }
}