﻿namespace Simple.Data.Ado
{
    using System;
    using System.Collections.Generic;

    internal static class ListExtensions
    {
        public static void SetWithBuffer<T>(this List<T> list, int index, T value)
        {
            if (list.Capacity > index)
            {
                while (list.Count < index)
                {
                    list.Add(default(T));
                }
                if (list.Count == index)
                {
                    list.Add(value);
                }
                else
                {
                    list[index] = value;
                }
            }
            else
            {
                if (list.Capacity < index)
                {
                    int newCapacity = list.Capacity;
                    while (newCapacity < index)
                    {
                        newCapacity *= 2;
                    }
                    list.Capacity = newCapacity;
                }
                while (list.Count < index)
                {
                    list.Add(default(T));
                }
                list.Add(value);
            }
        }
    }
}