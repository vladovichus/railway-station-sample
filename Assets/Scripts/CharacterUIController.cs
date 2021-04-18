using System;
using TMPro;
using UnityEngine;

namespace RailwayStationSample
{
    public class CharacterUIController : MonoBehaviour
    {
        [Header("UI")] 
        public TMP_Text Label;
        public TMP_Text IncreaseCountButtonLabel;
        public TMP_Text DecreaseCountButtonLabel;

        [Header("Counts")]
        public int IncreaseCount = 10;
        public int DecreaseCount = 10;

        [Header("Spawners")] 
        public CharacterSpawner[] Spawners;
        public Transform[] DestroyAreas;

        private void Update()
        {
            Label.text = $"Active characters: {Character.ActiveCharacters.Count}\nAll characters: {Character.AllCharacters.Count}";
            IncreaseCountButtonLabel.text = $"+{IncreaseCount}";
            DecreaseCountButtonLabel.text = $"–{DecreaseCount}";
        }

        [ContextMenu("Increase Character Count")]
        public void IncreaseCharacterCount()
        {
            for (int i = 0; i < IncreaseCount; i++)
            {
                Spawners.GetRandomElement().SpawnOneCharacter();
            }
        }
        
        [ContextMenu("Decrease Character Count")]
        public void DecreaseCharacterCount()
        {
            for (int i = 0; i < DecreaseCount; i++)
            {
                Character.ActiveCharacters.GetRandomElement().SendToDestroyArea(DestroyAreas.GetRandomElement().position);
            }
        }
    }
}