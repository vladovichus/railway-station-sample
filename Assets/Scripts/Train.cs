using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RailwayStationSample
{
    public class Train : MonoBehaviour
    {
        [Header("Parameters")] 
        public int Passengers;
        public int MaxPassengers = 20;
        
        [Header("Settings")] 
        public TrainMovingState State = TrainMovingState.Hidden;
        public float MinZ = -30f;
        public float MaxZ = 0f;

        [Header("Objects")]
        public GameObject TrainObject;
        public TMP_Text CountText;
        public Anchor[] EnterPoints;

        [Header("State Durations")] 
        public float MoveInDuration = 4f;
        public float MoveOutDuration = 4f;

        [Header("Events")] 
        public UnityEvent OnMoveInStarted;
        public UnityEvent OnMoveInFinished;
        public UnityEvent OnMoveOutStarted;
        public UnityEvent OnMoveOutFinished;
        public UnityEvent OnWaitingStarted;
        public UnityEvent OnWaitingFinished;

        private float _currentStateTime;
        private TrainMovingState _prevState;

        private void Reset()
        {
            TrainObject = GetComponentInChildren<Renderer>().transform.parent.gameObject;
            CountText = GetComponentInChildren<TMP_Text>();
            EnterPoints = GetComponentsInChildren<Anchor>();
        }

        private void Update()
        {
            _currentStateTime -= Time.deltaTime;

            foreach (var enterPoint in EnterPoints)
            {
                enterPoint.enabled = State == TrainMovingState.Waiting;
            }
            
            switch (State)
            {
                case TrainMovingState.Hidden:
                    TrainObject.SetActive(false);

                    break;
                case TrainMovingState.MoveIn:
                    TrainObject.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(MaxZ, MinZ, (_currentStateTime / MoveInDuration) * (_currentStateTime / MoveInDuration)));
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
                    float t = _currentStateTime / MoveOutDuration;
                    TrainObject.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(MinZ, MaxZ, (_currentStateTime / MoveOutDuration) * (_currentStateTime / MoveOutDuration)));
                    TrainObject.SetActive(true);

                    if (_currentStateTime <= 0)
                    {
                        State = TrainMovingState.Hidden;
                    }
                    
                    break;
            }

            RaiseEvents();
            _prevState = State;

            CountText.text = $"{Passengers} / {MaxPassengers}";
        }

        private void RaiseEvents()
        {
            if (_prevState == TrainMovingState.Hidden && State == TrainMovingState.MoveIn)
            {
                OnMoveInStarted?.Invoke();
            }
            
            if (_prevState == TrainMovingState.MoveIn && State == TrainMovingState.Waiting)
            {
                OnMoveInFinished?.Invoke();
            }
            
            if (_prevState == TrainMovingState.Waiting && State == TrainMovingState.MoveOut)
            {
                OnMoveOutStarted?.Invoke();
            }
            
            if (_prevState == TrainMovingState.MoveOut && State == TrainMovingState.Hidden)
            {
                OnMoveOutFinished?.Invoke();
            }

            if (_prevState != State && State == TrainMovingState.Waiting)
            {
                OnWaitingStarted?.Invoke();
            }

            if (_prevState == TrainMovingState.Waiting && State != _prevState)
            {
                OnWaitingFinished?.Invoke();
            }
        }

        [ContextMenu("Move in")]
        public void MoveIn()
        {
            gameObject.SetActive(true);
            State = TrainMovingState.MoveIn;
            _currentStateTime = MoveInDuration;
        }

        [ContextMenu("Move out")]
        public void MoveOut()
        {
            State = TrainMovingState.MoveOut;
            _currentStateTime = MoveOutDuration;
        }

        public void AddPassenger()
        {
            Passengers++;

            if (Passengers >= MaxPassengers)
            {
                MoveOut();
            }
        }

        public void ResetPassengers()
        {
            Passengers = 0;
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