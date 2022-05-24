using Domain.Models.General;
using Engine.Factories;

namespace Gunslinger.Types
{
	public class JavascriptDataTypeConversion
	{
		public static DataTypeInfo Convert_CSDataType_to_JSDataType(DataTypeInfo dataTypeInfo, Template template)
		{
			try
			{
				// have to do this manually, cloning can fail when type is abstract
				var clone = new DataTypeInfo
				{
					Name = dataTypeInfo.Name,
					IsNullable = dataTypeInfo.IsNullable,
					IsList = dataTypeInfo.IsList,
					IsTask = dataTypeInfo.IsTask,
					ListType = dataTypeInfo.ListType,
					Type = dataTypeInfo.Type,
					IsDictionary = dataTypeInfo.IsDictionary,
					KeyType = dataTypeInfo.KeyType
				};
				var jsType = getJSType(dataTypeInfo);
				clone.Name = NameFactory.Create(jsType, template, false);
				// js is probably just using a standard array list type
				// could make this configurable
				clone.ListType = "Array";
				return clone;
			}
			catch
			{
				throw;
			}
		}

		private static string getJSType(DataTypeInfo dataTypeInfo)
		{
			var lowerTypeName = dataTypeInfo.Name.Value.ToLower();
			switch (lowerTypeName)
			{
				case "datetime":
					return "Date";

				case "int":
				case "long":
				case "int32":
				case "int64":
				case "decimal":
				case "double":
				case "single":
					return "number";

				case "string":
					return "string";

				default:
					return dataTypeInfo.Name.Value;
			}
		}
	}
}