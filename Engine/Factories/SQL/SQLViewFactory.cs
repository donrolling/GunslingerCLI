using System.Collections.Generic;
using Engine.Models.SQL;
using Microsoft.SqlServer.Management.Smo;
using Models;

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