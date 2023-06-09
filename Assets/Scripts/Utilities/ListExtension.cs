﻿namespace Utilities
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class ListExtension
    {
        public static T RandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}