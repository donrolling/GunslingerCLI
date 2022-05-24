using Domain.Models;
using Microsoft.SqlServer.Management.Smo;
using Models.SQL;

namespace Gunslinger.Factories.SQL
{
	public class ViewInfoFactory
	{
		private static List<string> _excludedTypes = new List<string> { "dbo.sysdiagrams" };

		public static IEnumerable<View> Create(SQLServerInfo sqlServerInfo, GenerationContext settings, List<string> includeTheseEntitiesOnly, List<string> excludeTheseEntities)
		{
			//can't use linq expression here because TableCollection is gross
			var views = new List<View>();
			var whiteList = includeTheseEntitiesOnly.Any();
			var blackList = excludeTheseEntities.Any();
			foreach (View view in sqlServerInfo.Database.Views)
			{
				var uniqueName = UniqueNameFactory.Create(view.Schema, view.Name);
				if (_excludedTypes.Contains(uniqueName))
				{
					continue;
				}
				if (whiteList)
				{
					if (!includeTheseEntitiesOnly.Contains(uniqueName))
					{
						continue;
					}
				}
				if (blackList)
				{
					if (excludeTheseEntities.Contains(uniqueName))
					{
						continue;
					}
				}
				views.Add(view);
			}
			return views;
		}
	}
}