using Domain.Models.Configuration;
using Domain.Models.SQL;
using Omu.ValueInjecter;

namespace Engine.Factories.SQL
{
	public class SQLModelFactory
	{
		public static IEnumerable<SQLModel> Create(IEnumerable<SQLTable> sqlTables, GenerationContext generationSettings)
		{
			var result = new List<SQLModel>();
			foreach (var sqlTable in sqlTables)
			{
				result.Add(create(sqlTable, generationSettings));
			}
			return result;
		}

		public static SQLModel Create(SQLTable sqlTable, GenerationContext generationSettings)
		{
			return create(sqlTable, generationSettings);
		}

		private static SQLModel create(SQLTable sqlTable, GenerationContext generationSettings)
		{
			var sqlModel = new SQLModel();
			sqlModel.InjectFrom(sqlTable);
			sqlModel.Properties = sqlTable.Columns;
			if (
				generationSettings.AuditProperties == null
				|| !generationSettings.AuditProperties.Any()
			)
			{
				sqlModel.NonAuditNonKeyProperties = sqlModel
					.NonKeyProperties
					.ToList();
			}
			else
			{
				sqlModel.AuditNonKeyProperties = sqlModel
					.NonKeyProperties
					.Where(a =>
						generationSettings.AuditProperties.Contains(a.Name.Value)
					).ToList();
				sqlModel.NonAuditNonKeyProperties = sqlModel
					.NonKeyProperties
					.Where(a =>
						!generationSettings.AuditProperties.Contains(a.Name.Value)
					).ToList();
			}
			return sqlModel;
		}
	}
}