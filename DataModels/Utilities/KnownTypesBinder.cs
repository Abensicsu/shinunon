using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Utilities
{
    public class KnownTypesBinder : ISerializationBinder
    {
        private readonly Dictionary<string, Type> _knownTypes;

        public KnownTypesBinder(IEnumerable<Type> knownTypes)
        {
            _knownTypes = knownTypes.ToDictionary(t => t.FullName, t => t);
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            // Check if the type is in the known types
            if (_knownTypes.TryGetValue(typeName, out var type))
            {
                return type;
            }

            // If not found, fall back to the default behavior

            if (typeName.Contains("DataModels.Models"))
                return Type.GetType($"{typeName}, {assemblyName}");
            else
                throw new JsonSerializationException($"Unknown type: {typeName}");
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            // If the type is in the known types, use it
            if (_knownTypes.TryGetValue(serializedType.FullName, out var type))
            {
                assemblyName = type.Assembly.GetName().Name;
                typeName = type.FullName;
            }
            else
            {
                // If not, fall back to the default behavior
                assemblyName = serializedType.Assembly.GetName().Name;
                typeName = serializedType.FullName;
            }
        }
    }
}
