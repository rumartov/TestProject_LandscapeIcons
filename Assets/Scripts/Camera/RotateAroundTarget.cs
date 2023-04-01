using DefaultNamespace.StaticData;
using RTS_Cam;
using Services;
using UnityEngine;

namespace DefaultNamespace
{
    public sealed class RotateAroundTarget : MonoBehaviour
    {
        public float distanceToTarget;
        
        private IRaycastService _raycastService;
        private IInputService _inputService;

        private Camera _camera;
        private RTS_Camera _rtsCamera;

        private Vector3 _target;
        private Vector3 _previousPosition;

        public void Construct(IInputService inputService, IRaycastService raycastService)
        {
            _inputService = inputService;
            _raycastService = raycastService;
            
            _camera = Camera.main;
            _rtsCamera = _camera.GetComponent<RTS_Camera>();
        }

        void Update()
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
            
            if(HasTarget()) RotateAround();
            
        }

        private bool HasTarget()
        {
            return _target != Vector3.zero;
        }

        private bool RotationControlsInActive()
        {
            return _inputService.GetKeyUp(ControlConfig.KeyboardRotationKey) || _inputService.GetKeyUp(ControlConfig.MouseRotationKey);
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
            if (_inputService.GetKeyDown(ControlConfig.MouseRotationKey))
            {
                _previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (_inputService.GetKey(ControlConfig.MouseRotationKey))
            {
                Vector3 newPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
                Vector3 direction = _previousPosition - newPosition;

                float rotationAroundYAxis = -direction.x * 180;
                float rotationAroundXAxis = direction.y * 180;

                _camera.transform.position = _target;

                _camera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                _camera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

                _camera.transform.Translate(new Vector3(0, 0, -distanceToTarget));

                _previousPosition = newPosition;
            }
        }

        private bool RotationControlsActive()
        {
            return _inputService.GetKey(ControlConfig.KeyboardRotationKey) && _inputService.GetKey(ControlConfig.MouseRotationKey);
        }
    }
}

public static class ControlConfig
{
    public static KeyCode MouseRotationKey = KeyCode.Mouse1;
    public static KeyCode KeyboardRotationKey = KeyCode.LeftShift;
    public static float RotationTargetDistance = 10f;
}