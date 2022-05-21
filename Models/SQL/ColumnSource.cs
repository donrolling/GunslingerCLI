namespace Models.SQL
{
    public class ColumnSource : SQLKey
    {
        public string UniqueName { get; set; }
        public Name Table { get; set; }
        public string TablePlural { get; set; }
        public string Schema { get; set; }
    }
}