namespace EventHandler;

public static class DictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dict,
        TKey key,
        Func<TKey, TValue> valueFactory)
    {
        if (key is null) throw new ArgumentNullException(nameof(key));
        if (valueFactory is null) throw new ArgumentNullException(nameof(valueFactory));

        if (dict.ContainsKey(key))
            return dict[key];

        var value = valueFactory(key);
        dict.Add(key, value);

        return value;
    }

}