using System;
using UnityEngine;

namespace RailwayStationSample
{
    public class Anchor : MonoBehaviour
    {
        public float Weight;

        private void OnValidate()
        {
            if (Weight < 0)
            {
                Weight = 0;
            }
        }
    }
}