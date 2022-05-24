using Domain.Enums;
using Domain.Models.General;
using Domain.Models.SQL;
using Microsoft.SqlServer.Management.Smo;

namespace Engine.Factories.SQL
{
	public class SQLViewFactory
	{
		public static List<SQLView> Create(string @namespace, ProgrammingLanguage language, IEnumerable<View> smoViews, Template template)
		{
			return new List<SQLView>();
		}
	}
}