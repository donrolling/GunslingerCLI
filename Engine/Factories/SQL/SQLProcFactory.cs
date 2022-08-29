using Domain.Enums;
using Domain.Models.General;
using Domain.Models.SQL;
using Microsoft.SqlServer.Management.Smo;

using Omu.ValueInjecter;

namespace Engine.Factories.SQL
{
	public class SQLProcFactory
	{

		public static IEnumerable<SQLProc> Create(string @namespace, ProgrammingLanguage language, IEnumerable<StoredProcedure> smoProcs, Template template)
		{
			var xs = new List<SQLProc>();
			foreach (var proc in smoProcs)
			{
				var procResult = createProc(@namespace, language, proc, template);
				if (procResult.Failed)
				{
					continue;
				}
				xs.Add(procResult.Result);
			}
			return xs;
		}

		private static OperationResult<SQLProc> createProc(string @namespace, ProgrammingLanguage language, StoredProcedure proc, Template template)
		{
			if (string.IsNullOrEmpty(proc.Name))
			{
				throw new Exception("Table names must not be empty.");
			}
			var modelName = NameFactory.Create(proc.Name, template, true);
			var sqlColumns = getSQLParameters(modelName, language, proc.Parameters, template);
			var uniqueName = UniqueNameFactory.Create(proc.Schema, proc.Name);
			var entity = new SQLProc
			{
				//TableName = NameFactory.Create(table.Name, template, false), // tablename does not represent a class
				//UniqueName = uniqueName,
				//Name = NameFactory.Create(table.Name, template, true), // name does represent a class
				//Schema = table.Schema,
				//Key = sqlKeys.FirstOrDefault(),
				//Keys = sqlKeys,
				//Columns = sqlColumns,
				//Namespace = @namespace
			};

			var sqlTable = new SQLProc();
			sqlTable.InjectFrom(entity);
			return OperationResult.Ok(sqlTable);
		}

		private static object getSQLParameters(Name modelName, ProgrammingLanguage language, StoredProcedureParameterCollection parameters, Template template)
		{
			var result = new List<SQLColumn>();
			foreach (var column in parameters)
			{
				var property = SQLParameterFactory.Create(modelName, column, language, template);
				result.Add(property);
			}
			return result;
		}
	}
}