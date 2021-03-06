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
            
            if (IsFinished())
            {
                _cooldownTime -= Time.deltaTime;

                if (TargetAnchor != null && Vector3.Distance(transform.position, TargetAnchor.transform.position) < 2f * Agent.stoppingDistance)
                {
                    TargetAnchor?.Raise(this);
                }
                
                TargetAnchor = null;
            }

            if (_cooldownTime <= 0 || TargetAnchor != null && !TargetAnchor.enabled)
            {
                _cooldownTime = Random.Range(MinChangeTargetWaitingDuration, MaxChangeTargetWaitingDuration);
                SetRandomTargetAnchor();
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
        public void SendToDestroyArea(Vector3 targetPosition)
        {
            _isActive = false;
            ActiveCharacters.Remove(this);
            
            Agent.SetDestination(targetPosition);
        }

        public void Destroy()
        {
            Destroy(gameObject);
            ActiveCharacters.Remove(this);
            AllCharacters.Remove(this);
        }

        private bool IsFinished() => Agent.hasPath && Agent.remainingDistance <= Agent.stoppingDistance + 0.1f;
    }
}