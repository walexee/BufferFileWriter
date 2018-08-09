using System.Collections.Generic;

namespace FileBlockUpload.Tests
{
    public static class SettingsExtensions
    {
        public static IDictionary<string, object> ToKeyValuePairs<T>(this T source, string keyPrefix = null)
            where T : class
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var keyValuePairs = new Dictionary<string, object>();

            foreach (var prop in props)
            {
                if (prop.CanRead)
                {
                    keyValuePairs[$"{keyPrefix}{prop.Name}"] = prop.GetValue(source);
                }
            }

            return keyValuePairs;
        }

        public static T FromKeyValuePairs<T>(this IDictionary<string, object> keyValuePairs, string keyPrefix = null)
            where T : new()
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var result = new T();

            foreach (var prop in props)
            {
                var key = $"{keyPrefix}{prop.Name}";

                if (prop.CanWrite && keyValuePairs.ContainsKey(key))
                {
                    // TODO: consider if the keyvalue has string value instead of the actual type
                    var value = keyValuePairs[key];

                    prop.SetValue(result, value);
                }
            }

            return result;
        }
    }
}
