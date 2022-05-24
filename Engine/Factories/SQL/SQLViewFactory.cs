using System.Collections.Generic;
using Domain.Models.General;
using Domain.Models.SQL;
using Microsoft.SqlServer.Management.Smo;

namespace Gunslinger.Factories.SQL
{
	public class SQLViewFactory
    {
        public static List<SQLView> Create(string @namespace, Models.Enums.Language language, IEnumerable<View> smoViews, Template template)
        {
            return new List<SQLView>();
        }
    }
}