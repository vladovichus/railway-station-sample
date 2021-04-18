using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RailwayStationSample
{
    public class Anchor : MonoBehaviour
    {
        private static List<Anchor> ActiveAnchors { get; } = new List<Anchor>();
        
        public float Weight;

        public event Action<Character> OnCharacterReached;

        private void OnValidate()
        {
            if (Weight < 0)
            {
                Weight = 0;
            }
        }

        private void OnEnable()
        {
            ActiveAnchors.Add(this);
        }

        private void OnDisable()
        {
            ActiveAnchors.Remove(this);
        }

        public void Raise(Character sender)
        {
            OnCharacterReached?.Invoke(sender);
        }
        
        public static Anchor GetRandomAnchor(bool useWeights = true)
        {
            if (!useWeights)
            {
                return ActiveAnchors[Random.Range(0, ActiveAnchors.Count - 1)];
            }

            float weightsSum = ActiveAnchors.Select(x => x.Weight).Sum();
            float randomWeight = Random.Range(0, weightsSum);

            float maxWeight = weightsSum;
            float minWeight = weightsSum;
            foreach (Anchor anchor in ActiveAnchors)
            {
                minWeight -= anchor.Weight;
                if (randomWeight > minWeight && randomWeight <= maxWeight)
                {
                    return anchor;
                }

                maxWeight = minWeight;
            }

            return ActiveAnchors.FirstOrDefault();
        }
    }
}