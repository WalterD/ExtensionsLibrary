using System;
using System.Collections.Generic;

namespace ExtensionsLibrary
{
    public static class ArrayExtensions
    {
        public static List<T> ToList<T>(this Array arr)
        {
            return new List<T>((T[])arr);
        }
    }
}
