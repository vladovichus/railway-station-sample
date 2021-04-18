using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace RailwayStationSample
{
    public class Character : MonoBehaviour
    {
        public static List<Character> ActiveCharacters = new List<Character>();
        public static List<Character> AllCharacters = new List<Character>();
        
        [Header("Settings")]
        public float MinChangeTargetWaitingDuration = 1f;
        public float MaxChangeTargetWaitingDuration = 5f;
        
        [Header("Components")]
        public NavMeshAgent Agent;
        public Transform[] DestroyAreas;
        
        [Header("Target")]
        public Anchor TargetAnchor;

        private float _cooldownTime;
        private bool _isActive;

        private void Reset()
        {
            Agent = GetComponentInChildren<NavMeshAgent>();
        }

        private void Awake()
        {
            _isActive = true;
            
            ActiveCharacters.Add(this);
            AllCharacters.Add(this);
        }

        private void OnDestroy()
        {
            ActiveCharacters.Remove(this);
            AllCharacters.Remove(this);
        }

        private void Start()
        {
            SetRandomTargetAnchor();
        }

        private void Update()
        {
            if (!_isActive)
            {
                if (IsFinished())
                {
                    Destroy(gameObject);
                }
                
                return;
            }

            if (_cooldownTime <= 0 || TargetAnchor != null && !TargetAnchor.enabled)
            {
                _cooldownTime = Random.Range(MinChangeTargetWaitingDuration, MaxChangeTargetWaitingDuration);
                SetRandomTargetAnchor();
            }
            
            if (IsFinished())
            {
                _cooldownTime -= Time.deltaTime;
                TargetAnchor?.Raise(this);
                TargetAnchor = null;
            }
        }

        private void SetRandomTargetAnchor()
        {
            SetTargetAnchor(Anchor.GetRandomAnchor());
        }

        private void SetTargetAnchor(Anchor targetAnchor)
        {
            TargetAnchor = targetAnchor;

            Agent.SetDestination(TargetAnchor.transform.position);
        }

        [ContextMenu("Send to Destroy Area")]
        public void SendToDestroyArea()
        {
            _isActive = false;
            ActiveCharacters.Remove(this);

            Transform randomDestroyArea = DestroyAreas[Random.Range(0, DestroyAreas.Length - 1)];

            Agent.SetDestination(randomDestroyArea.position);
        }

        private bool IsFinished() => !Agent.hasPath || Agent.hasPath && Agent.remainingDistance <= Agent.stoppingDistance + 1f;
    }
}