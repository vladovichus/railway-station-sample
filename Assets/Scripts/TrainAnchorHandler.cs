using System;
using UnityEngine;

namespace RailwayStationSample
{
    [RequireComponent(typeof(Anchor))]
    public class TrainAnchorHandler : MonoBehaviour
    {
        public Train Train;

        public Anchor Anchor;

        private void Reset()
        {
            Train = GetComponentInParent<Train>();
            Anchor = GetComponentInChildren<Anchor>();
        }

        private void OnEnable()
        {
            Anchor.OnCharacterReached += AddCharacterToTrain;
        }

        private void OnDisable()
        {
            Anchor.OnCharacterReached -= AddCharacterToTrain;
        }

        private void AddCharacterToTrain(Character character)
        {
            if (Train.Passengers >= Train.MaxPassengers)
            {
                return;
            }
            
            character.Destroy();
            Train.AddPassenger();
        }
    }
}