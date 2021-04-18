using System.Collections.Generic;
using UnityEngine;

namespace RailwayStationSample
{
    public static class ArrayEx 
    {
        public static T GetRandomElement<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length - 1)];
        }
        
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count - 1)];
        }
    }
}