using Domain.Models.Configuration;
using Domain.Models.SQL;
using Microsoft.SqlServer.Management.Smo;

namespace Engine.Factories.SQL
{
	public class TableInfoFactory
	{
		private static List<string> _excludedTypes = new List<string> { "dbo.sysdiagrams" };

		public static IEnumerable<Table> Create(SQLServerInfo sqlServerInfo, GenerationContext settings, List<string> includeTheseEntitiesOnly, List<string> excludeTheseEntities)
		{
			//can't use linq expression here because TableCollection is gross
			var tables = new List<Table>();
			var whiteList = includeTheseEntitiesOnly.Any();
			var blackList = excludeTheseEntities.Any();
			foreach (Table table in sqlServerInfo.Database.Tables)
			{
				if (_excludedTypes.Contains(table.Name))
				{
					continue;
				}
				if (whiteList)
				{
					if (!includeTheseEntitiesOnly.Contains(table.Name))
					{
						continue;
					}
				}
				if (blackList)
				{
					if (excludeTheseEntities.Contains(table.Name))
					{
						continue;
					}
				}
				tables.Add(table);
			}
			return tables;
		}
	}
}