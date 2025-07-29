using System.Collections;
using System.Reflection;

namespace AspireDbAndCache.Api.Utility
{
    public static class HashCodeGenerator
    {
        /// <summary>
        /// Versione più robusta che gestisce collezioni e oggetti annidati
        /// </summary>
        public static int GetObjectHashCode<T>(T obj)
        {
            return GetDeepHashCodeInternal(obj, new HashSet<object>());
        }

        private static int GetDeepHashCodeInternal(object obj, HashSet<object> visited)
        {
            if (obj == null) return 0;

            // Evita cicli infiniti
            if (visited.Contains(obj)) return obj.GetHashCode();
            visited.Add(obj);

            Type type = obj.GetType();

            // Per tipi primitivi e string usa l'hash code nativo
            if (type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal))
            {
                return obj.GetHashCode();
            }

            unchecked
            {
                int hash = 17;

                // Gestisce le collezioni
                if (obj is IEnumerable enumerable && !(obj is string))
                {
                    foreach (object item in enumerable)
                    {
                        hash = hash * 31 + GetDeepHashCodeInternal(item, visited);
                    }
                    return hash;
                }

                // Gestisce oggetti complessi
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    if (property.CanRead && property.GetIndexParameters().Length == 0) // Esclude indexer
                    {
                        object value = property.GetValue(obj);
                        hash = hash * 31 + GetDeepHashCodeInternal(value, visited);
                    }
                }

                return hash;
            }
        }
    }
}
