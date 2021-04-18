using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace RailwayStationSample
{
    public class Character : MonoBehaviour
    {
        [Header("Settings")]
        public float MinChangeTargetWaitingDuration = 1f;
        public float MaxChangeTargetWaitingDuration = 5f;
        
        [Header("Components")]
        public NavMeshAgent Agent;
        
        [Header("Target")]
        public Anchor TargetAnchor;

        private float _cooldownTime;

        private void Reset()
        {
            Agent = GetComponentInChildren<NavMeshAgent>();
        }

        private void Start()
        {
            SetRandomTargetAnchor();
        }

        private void Update()
        {
            if (IsFinished())
            {
                _cooldownTime -= Time.deltaTime;
            }

            if (_cooldownTime <= 0)
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

        private bool IsFinished() => !Agent.hasPath || Agent.hasPath && Agent.remainingDistance <= Agent.stoppingDistance + 1f;
    }
}