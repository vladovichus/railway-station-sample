using UnityEngine;
using UnityEngine.Events;

namespace RailwayStationSample
{
    public class Train : MonoBehaviour
    {
        [Header("Settings")] 
        public Train OtherTrain;
        public TrainMovingState State = TrainMovingState.Hidden;
        public float MinZ = -30f;
        public float MaxZ = 0f;

        [Header("Objects")]
        public GameObject TrainObject;

        [Header("State Durations")] 
        public float MoveInDuration = 4f;
        public float MoveOutDuration = 4f;

        [Header("Events")] 
        public UnityEvent OnWaitingStarted;

        private float _currentStateTime;

        private void Reset()
        {
            TrainObject = GetComponentInChildren<Renderer>().transform.parent.gameObject;
        }

        private void Update()
        {
            _currentStateTime -= Time.deltaTime;
            
            switch (State)
            {
                case TrainMovingState.Hidden:
                    TrainObject.SetActive(false);

                    break;
                case TrainMovingState.MoveIn:
                    TrainObject.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(MaxZ, MinZ, _currentStateTime * _currentStateTime / MoveInDuration));
                    TrainObject.SetActive(true);

                    if (_currentStateTime <= 0)
                    {
                        State = TrainMovingState.Waiting;
                        OnWaitingStarted?.Invoke();
                    }
                    
                    break;
                case TrainMovingState.Waiting:
                    TrainObject.transform.localPosition = Vector3.zero;
                    TrainObject.SetActive(true);
                    
                    break;
                case TrainMovingState.MoveOut:
                    TrainObject.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(MinZ, MaxZ, _currentStateTime * _currentStateTime / MoveInDuration));
                    TrainObject.SetActive(true);

                    if (_currentStateTime <= 0)
                    {
                        State = TrainMovingState.Hidden;
                    }
                    
                    break;
            }
        }

        [ContextMenu("Move in")]
        public void MoveIn()
        {
            State = TrainMovingState.MoveIn;
            _currentStateTime = MoveInDuration;
        }

        [ContextMenu("Move out")]
        public void MoveOut()
        {
            State = TrainMovingState.MoveOut;
            _currentStateTime = MoveOutDuration;
        }
        
        public enum TrainMovingState
        {
            Hidden,
            MoveIn,
            Waiting,
            MoveOut
        }
    }
}