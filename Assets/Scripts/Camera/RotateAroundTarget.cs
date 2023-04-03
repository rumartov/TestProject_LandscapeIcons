using RTS_Cam;
using Services.Input;
using Services.Raycast;
using UnityEngine;

namespace Camera
{
    public sealed class RotateAroundTarget : MonoBehaviour
    {
        public float distanceToTarget;
        public KeyCode mouseRotationKey;
        public KeyCode keyboardRotationKey;

        private UnityEngine.Camera _camera;
        private IInputService _inputService;
        private Vector3 _previousPosition;

        private IRaycastService _raycastService;
        private RTS_Camera _rtsCamera;

        private Vector3 _target;

        private void Update()
        {
            if (RotationControlsActive())
            {
                GetTargetPoint();
                _rtsCamera.enabled = false;
            }

            if (RotationControlsInActive())
            {
                ResetTarget();
                _rtsCamera.enabled = true;
            }

            if (HasTarget()) RotateAround();
        }

        public void Construct(IInputService inputService, IRaycastService raycastService)
        {
            _inputService = inputService;
            _raycastService = raycastService;

            _camera = UnityEngine.Camera.main;
            _rtsCamera = _camera.GetComponent<RTS_Camera>();
        }

        private bool HasTarget()
        {
            return _target != Vector3.zero;
        }

        private bool RotationControlsInActive()
        {
            return _inputService.GetKeyUp(keyboardRotationKey) || _inputService.GetKeyUp(mouseRotationKey);
        }

        private void ResetTarget()
        {
            _target = Vector3.zero;
        }

        private void GetTargetPoint()
        {
            var cameraTransform = _camera.transform;
            var position = cameraTransform.position;
            var direction = cameraTransform.forward;

            var ray = new Ray(position, direction);

            Debug.DrawRay(position, direction, Color.blue, 4f);

            if (_raycastService.PhysicsRaycast(ray, out var hit))
            {
                Debug.Log(hit.point);
                _target = hit.point;
            }
        }

        private void RotateAround()
        {
            if (_inputService.GetKeyDown(mouseRotationKey))
            {
                _previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (_inputService.GetKey(mouseRotationKey))
            {
                var newPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
                var direction = _previousPosition - newPosition;

                var rotationAroundYAxis = -direction.x * 180;
                var rotationAroundXAxis = direction.y * 180;

                _camera.transform.position = _target;

                _camera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                _camera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

                _camera.transform.Translate(new Vector3(0, 0, -distanceToTarget));

                _previousPosition = newPosition;
            }
        }

        private bool RotationControlsActive()
        {
            return _inputService.GetKey(keyboardRotationKey) && _inputService.GetKey(mouseRotationKey);
        }
    }
}