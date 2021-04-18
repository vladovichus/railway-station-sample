using UnityEngine;
using UnityEngine.Events;

namespace RailwayStationSample
{
    public class Bus : MonoBehaviour
    {
        [Header("Settings")]
        public BusMovingState State = BusMovingState.Hidden;
        public float MinZ = -25f;
        public float MaxZ = 25f;

        [Header("Objects")]
        public GameObject BusObject;

        [Header("State Durations")] 
        public float HiddenStateMinDuration = 10f;
        public float HiddenStateMaxDuration = 20f;
        public float MoveInDuration = 4f;
        public float MoveOutDuration = 4f;
        public float WaitingStateMinDuration = 5f;
        public float WaitingStateMaxDuration = 10f;

        [Header("Events")] 
        public UnityEvent OnWaitingStarted;

        private float _currentStateTime;

        private void Reset()
        {
            BusObject = GetComponentInChildren<Renderer>().gameObject;
        }

        private void Update()
        {
            _currentStateTime -= Time.deltaTime;
            
            switch (State)
            {
                case BusMovingState.Hidden:
                    BusObject.SetActive(false);

                    if (_currentStateTime <= 0)
                    {
                        State = BusMovingState.MoveIn;
                        _currentStateTime = MoveInDuration;
                    }

                    break;
                case BusMovingState.MoveIn:
                    BusObject.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(0, MinZ, _currentStateTime * _currentStateTime / MoveInDuration));
                    BusObject.SetActive(true);

                    if (_currentStateTime <= 0)
                    {
                        State = BusMovingState.Waiting;
                        _currentStateTime = Random.Range(WaitingStateMinDuration, WaitingStateMaxDuration);
                        OnWaitingStarted?.Invoke();
                    }
                    
                    break;
                case BusMovingState.Waiting:
                    BusObject.transform.localPosition = Vector3.zero;
                    BusObject.SetActive(true);

                    if (_currentStateTime <= 0)
                    {
                        State = BusMovingState.MoveOut;
                        _currentStateTime = MoveOutDuration;
                    }
                    
                    break;
                case BusMovingState.MoveOut:
                    BusObject.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(MaxZ, 0, _currentStateTime * _currentStateTime / MoveInDuration));
                    BusObject.SetActive(true);

                    if (_currentStateTime <= 0)
                    {
                        State = BusMovingState.Hidden;
                        _currentStateTime = Random.Range(HiddenStateMinDuration, HiddenStateMaxDuration);
                    }
                    
                    break;
            }
        }

        

        public enum BusMovingState
        {
            Hidden,
            MoveIn,
            Waiting,
            MoveOut
        }
    }
}