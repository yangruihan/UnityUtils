using System.Collections.Generic;

namespace MyNamespace.Utils
{
    public static class ListHelper
    {
        public static void ConvertListToArr<T>(this List<T> list, ref T[] arr)
        {
            if (list == null)
            {
                arr = null;
                return;
            }

            arr = list.ToArray();
        }

        public static void TryRemove<T>(this List<T> list, T data)
        {
            if (list == null || list.Count == 0)
                return;

            var index = list.IndexOf(data);
            if (index >= 0)
                list.RemoveAt(index);
        }
    }
}