using GameEvents;
using GameEvents.Actions;
using UnityEngine;

namespace PlayerCamera
{
    class CameraController : MonoBehaviour
    {
        protected Camera Camera;

        public float NormalPanSpeed = 50f;
        public float FastPanSpeed = 150f;
        protected bool IsPanning;
        protected Vector3 PanVelocity;
        protected bool PanUp;
        protected bool PanDown;
        protected bool PanRight;
        protected bool PanLeft;

        private void Awake()
        {
            GameManager.EventSystem.Subscribe(typeof(CameraPanRequested), HandleCameraPan);
            Camera = gameObject.GetComponent<Camera>();
        }

        protected void HandleCameraPan(IEvent @event)
        {
            if (!(@event is CameraPanRequested))
            {
                return;
            }

            CameraPanRequested panEvent = @event as CameraPanRequested;
            bool panStart = panEvent.Request == CameraPanRequested.RequestType.Start;
            float panSpeed = panEvent.FastSpeed ? FastPanSpeed : NormalPanSpeed;
            switch (panEvent.Dir)
            {
                case CameraPanRequested.Direction.Up:
                    PanUp = panStart;
                    PanVelocity.y += panEvent.Request == CameraPanRequested.RequestType.Start ? panSpeed : -panSpeed;
                    break;
                case CameraPanRequested.Direction.Down:
                    PanDown = panStart;
                    PanVelocity.y += panEvent.Request == CameraPanRequested.RequestType.Start ? -panSpeed : panSpeed;
                    break;
                case CameraPanRequested.Direction.Right:
                    PanRight = panStart;
                    PanVelocity.x += panEvent.Request == CameraPanRequested.RequestType.Start ? panSpeed : -panSpeed;
                    break;
                case CameraPanRequested.Direction.Left:
                    PanLeft = panStart;
                    PanVelocity.x += panEvent.Request == CameraPanRequested.RequestType.Start ? -panSpeed : panSpeed;
                    break;
            }
            IsPanning = PanUp || PanDown || PanRight || PanLeft;
            PanVelocity = IsPanning ? Vector3.ClampMagnitude(PanVelocity, panSpeed) : Vector3.zero;
        }

        private void Update()
        {
            PanCamera();
        }

        private void PanCamera()
        {
            if (!IsPanning)
            {
                return;
            }

            Camera.transform.Translate(PanVelocity * Time.deltaTime);
        }
    }
}
