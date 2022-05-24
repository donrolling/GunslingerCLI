namespace Engine.Factories.SQL
{
	public class UniqueNameFactory
	{
		public static string Create(string schema, string tableName)
		{
			return $"{ schema }.{ tableName }";
		}
	}
}