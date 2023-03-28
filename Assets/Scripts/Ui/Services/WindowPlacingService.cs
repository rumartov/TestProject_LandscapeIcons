using Services;
using UnityEngine;

namespace Ui.Services
{
    public class WindowPlacingService : IWindowPlacingService
    {
        private readonly Camera _camera;
        private readonly IGameFactory _factory;
        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;
        private readonly UpdateService _updateService;

        public WindowPlacingService(IInputService inputService, IGameFactory factory, IRaycastService raycastService)
        {
            _factory = factory;
            _raycastService = raycastService;
            _inputService = inputService;
            _camera = Camera.main;

            IsPlacing = false;

            _inputService.OnMouseClick += PlaceWindowIcon;
        }

        public bool IsPlacing { get; set; }

        public void PlaceWindowIcon()
        {
            if (IsPlacing)
                if (_inputService.LeftMouseDown())
                {
                    var ray = _camera.ScreenPointToRay(_inputService.MousePosition());
                    Debug.DrawRay(_camera.transform.position, _inputService.MousePosition(), Color.blue, 4f);
                    if (_raycastService.PhysicsRaycast(ray, out var hit))
                    {
                        var spawnPosition = WindowIconInitPoint(hit);
                        _factory.CreateWindowIcon(spawnPosition);
                    }
                }
        }

        private Vector3 WindowIconInitPoint(RaycastHit hit)
        {
            return hit.point + Vector3.up * 2;
        }
    }
}