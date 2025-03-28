using System.Runtime.InteropServices;

namespace JopSchemaEditor
{
    internal static class DictionaryUtils
    {
        /// <summary>
        /// Adds or updates a value in a dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to add or update the value in.</param>
        /// <param name="key">The key of the value to add or update.</param>
        /// <param name="value">The value to add or update.</param>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull
        {
            ref TValue? val = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out _);
            val = value;
        }

        
    }
}
