using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RailwayStationSample
{
    public class CharacterSpawner : MonoBehaviour
    {
        public GameObject CharacterPrefab;
        
        public int MinSpawnCount = 1;
        public int MaxSpawnCount = 5;

        private int _nameIndexCounter;
        
        [ContextMenu("Spawn")]
        public void SpawnRandomCharacters()
        {
            int spawnCount = Random.Range(MinSpawnCount, MaxSpawnCount);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnOneCharacter();   
            }
        }

        public void SpawnOneCharacter()
        {
            GameObject go = Instantiate(CharacterPrefab, transform.position, Quaternion.identity);

            go.name = $"Character (Bus #{_nameIndexCounter})";
            _nameIndexCounter++;
        }
    }
}