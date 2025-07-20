using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
    {
        int index = 0;
        foreach (var item in enumerable)
        {
            action(item, index++);
        }
    }

    public static void For<T>(this IList<T> list, Action<T> action)
    {
        for (var i = 0; i < list.Count; i++)
        {
            action(list[i]);
        }
    }

    public static void For<T>(this IList<T> list, Action<T, int> action)
    {
        for (var i = 0; i < list.Count; i++)
        {
            action(list[i], i);
        }
    }

    public static IEnumerable<T> Yield<T>(this T obj)
    {
        yield return obj;
    }

    public static IEnumerable<T> YieldWith<T>(this T obj, params T[] objects)
    {
        yield return obj;
        foreach (var additionalObj in objects)
        {
            yield return additionalObj;
        }
    }

    public static IEnumerable<T> YieldWith<T>(this T obj, IEnumerable<T> objects)
    {
        yield return obj;
        foreach (var additionalObj in objects)
        {
            yield return additionalObj;
        }
    }

    public static T MinBy<T, N>(this IEnumerable<T> enumerable, Func<T, N> selector) where N : IComparable<N>
    {
        if (!enumerable.Any())
        {
            return default;
        }

        var minItem = enumerable.First();
        var minValue = selector(minItem);
        N valueToCompare;
        foreach (var item in enumerable)
        {
            valueToCompare = selector(item);
            if (valueToCompare.CompareTo(minValue) < 0)
            {
                minValue = valueToCompare;
                minItem = item;
            }
        }
        return minItem;
    }

    public static T MaxBy<T, N>(this IEnumerable<T> enumerable, Func<T, N> selector) where N : IComparable<N>
    {
        if (!enumerable.Any())
        {
            return default;
        }

        var maxItem = enumerable.First();
        var maxValue = selector(maxItem);
        N valueToCompare;
        foreach (var item in enumerable)
        {
            valueToCompare = selector(item);
            if (valueToCompare.CompareTo(maxValue) > 0)
            {
                maxValue = valueToCompare;
                maxItem = item;
            }
        }
        return maxItem;
    }

    public static IEnumerable<T> ExceptNotNull<T>(this IEnumerable<T> enumerable, IEnumerable<T> toExcept)
    {
        return toExcept == null ? enumerable : enumerable.Except(toExcept);
    }

    public static IEnumerable<T> SkipNull<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.Where(x => x != null);
    }

    public static void AddOrReplaceRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> toAdd)
    {
        foreach (var kv in toAdd)
        {
            if (dictionary.ContainsKey(kv.Key))
            {
                dictionary[kv.Key] = kv.Value;
            }
            else
            {
                dictionary.Add(kv.Key, kv.Value);
            }
        }
    }

    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey toGet, TValue defaultValue = default)
    {
        if (dictionary.TryGetValue(toGet, out var value))
        {
            return value;
        }
        return defaultValue;
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey toGet, Func<TValue> factory)
    {
        if (dictionary.TryGetValue(toGet, out var value))
        {
            return value;
        }
        else
        {
            value = factory();
            dictionary[toGet] = value;
        }
        return value;
    }

    public static void RemoveRangeByKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> toRemove)
    {
        foreach (var val in toRemove)
        {
            dictionary.Remove(val);
        }
    }

    public static Dictionary<K, V> AddOrModify<K, V>(this Dictionary<K, V> dictionary, K key, Func<V> valueCreator, Func<V, V> valueModifier)
    {
        if (dictionary.TryGetValue(key, out var value))
        {
            dictionary[key] = valueModifier(value);
        }
        else
        {
            dictionary.Add(key, valueCreator());
        }

        return dictionary;
    }

    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> toAdd)
    {
        foreach (var val in toAdd)
        {
            set.Add(val);
        }
    }

    public static void RemoveRange<T>(this HashSet<T> set, IEnumerable<T> toRemove)
    {
        foreach (var val in toRemove)
        {
            set.Remove(val);
        }
    }

    public static void RemoveRange<T>(this List<T> list, IEnumerable<T> toRemove)
    {
        foreach (var val in toRemove)
        {
            list.Remove(val);
        }
    }

    public static void AddRange<T>(this List<T> list, params T[] values)
    {
        list.AddRange(values);
    }
}
