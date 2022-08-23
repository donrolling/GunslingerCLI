using Domain.Models.Configuration;
using Domain.Models.SQL;
using Microsoft.SqlServer.Management.Smo;

namespace Engine.Factories.SQL
{
	public class ProcInfoFactory
	{
		private static List<string> _excludedTypes = new List<string> { };
		private static List<string> _excludedSchemas = new List<string> { "cdc", "sys" };

		public static IEnumerable<StoredProcedure> Create(
			SQLServerInfo sqlServerInfo,
			GenerationContext settings,
			List<string> includeTheseEntitiesOnly,
			List<string> excludeTheseEntities
		)
		{
			var xs = new List<StoredProcedure>();
			var whiteList = includeTheseEntitiesOnly.Any();
			var blackList = excludeTheseEntities.Any();
			foreach (StoredProcedure storedProcedure in sqlServerInfo.Database.StoredProcedures)
			{
				if (_excludedTypes.Contains(storedProcedure.Name))
				{
					continue;
				}
				if (_excludedSchemas.Contains(storedProcedure.Schema))
				{
					continue;
				}
				if (whiteList)
				{
					if (!includeTheseEntitiesOnly.Contains(storedProcedure.Name))
					{
						continue;
					}
				}
				if (blackList)
				{
					if (excludeTheseEntities.Contains(storedProcedure.Name))
					{
						continue;
					}
				}
				xs.Add(storedProcedure);
			}
			return xs;
		}
	}
}