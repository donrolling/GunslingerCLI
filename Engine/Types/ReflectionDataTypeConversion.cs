using Engine.Models;
using Gunslinger.Factories;
using Models;

namespace Engine.Types
{
    public static class ReflectionDataTypeConversion
    {
        private static readonly List<string> _standardTypeNames = new List<string> {
            "char",
            "decimal",
            "double",
            "single",
            "string",
            "bool",
            "sbyte",
            "byte"
        };

        // this converts from c# reflection to c# like a human would write it
        // totally incomplete
        public static DataTypeInfo Convert_ReflectionDataType_to_CSDataType(Type type, Template template)
        {
            var isCollection = type.IsGenericType &&
                (
                    typeof(List<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                    ||
                    typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                    ||
                    typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                    ||
                    typeof(ICollection<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                );
            if (isCollection)
            {
                return getListDataTypeInfo(type, template);
            }
            var isDictionary = type.IsGenericType &&
                 (
                    typeof(Dictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition())
                    ||
                    typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition())
                );

            if (isDictionary)
            {
                return getDictionaryDataTypeInfo(type, template);
            }
            return getBasicTypeInfo(type, template);
        }

        private static DataTypeInfo getDictionaryDataTypeInfo(Type type, Template template)
        {
            var dictionaryType = string.Empty;
            if (typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                dictionaryType = "IDictionary";
            }
            if (typeof(Dictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                dictionaryType = "Dictionary";
            }

            if (string.IsNullOrEmpty(dictionaryType))
            {
                throw new Exception("Couldn't figure out the dictionary type.");
            }

            var keyType = type.GetGenericArguments()[0];
            var underlyingType = type.GetGenericArguments()[0];
            var keyDataTypeInfo = Convert_ReflectionDataType_to_CSDataType(keyType, template);
            var valueDataTypeInfo = Convert_ReflectionDataType_to_CSDataType(underlyingType, template);
            valueDataTypeInfo.KeyType = keyDataTypeInfo;
            valueDataTypeInfo.IsDictionary = true;
            valueDataTypeInfo.ListType = dictionaryType;
            return valueDataTypeInfo;
        }

        private static DataTypeInfo getBasicTypeInfo(Type type, Template template)
        {
            // arrays
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var basicTypeInfo = getBasicTypeInfo(elementType, template);
                basicTypeInfo.IsList = true;
                basicTypeInfo.ListType = "Array";
                return basicTypeInfo;
            }

            var typeName = type.Name;
            switch (typeName)
            {
                case "Nullable`1":
                    return getNullableDataTypeInfo(type, template);

                // simply remove task, because it should be resolved for output consumers
                case "Task`1":
                case "TaskFactory`1":
                    return getTaskDataTypeInfo(type, template);

                default:
                    return new DataTypeInfo
                    {
                        Name = NameFactory.Create(convertBasicType(typeName), template, false),
                        Type = type
                    };
            }
        }

        private static DataTypeInfo getTaskDataTypeInfo(Type type, Template template)
        {
            var taskResultType = type.GetGenericArguments()[0];
            var result = Convert_ReflectionDataType_to_CSDataType(taskResultType, template);
            result.IsTask = true;
            return result;
        }

        private static DataTypeInfo getListDataTypeInfo(Type type, Template template)
        {
            var underlyingType = type.GetGenericArguments()[0];
            var dataTypeInfo = Convert_ReflectionDataType_to_CSDataType(underlyingType, template);
            dataTypeInfo.IsList = true;
            var listType = getListType(type);
            dataTypeInfo.ListType = listType;
            return dataTypeInfo;
        }

        private static string getListType(Type type)
        {
            if (typeof(List<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                return "List";
            }
            if (typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                return "IEnumerable";
            }
            if (typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                return "IList";
            }
            if (typeof(ICollection<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                return "ICollection";
            }
            throw new Exception("Couldn't figure out what collection type this is.");
        }

        private static DataTypeInfo getNullableDataTypeInfo(Type type, Template template)
        {
            var underlyingTypeName = Nullable.GetUnderlyingType(type).Name;
            var dataTypeName = convertBasicType(underlyingTypeName);
            return new DataTypeInfo
            {
                IsNullable = true,
                Name = NameFactory.Create(dataTypeName, template, false),
                Type = type
            };
        }

        // potentially incomplete
        private static string convertBasicType(string typeName)
        {
            var typeNameLower = typeName.ToLower();
            if (_standardTypeNames.Contains(typeNameLower))
            {
                return typeNameLower;
            }
            switch (typeName)
            {
                case "Int32":
                    return "int";

                case "UInt32":
                    return "uint";

                case "Int16":
                    return "short";

                case "UInt16":
                    return "ushort";

                case "Int64":
                    return "long";

                case "UInt64":
                    return "ulong";

                case "Boolean":
                    return "bool";

                default:
                    return typeName;
            }
        }
    }
}