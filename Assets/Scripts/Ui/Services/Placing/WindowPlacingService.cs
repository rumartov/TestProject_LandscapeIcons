using Services;
using Services.Factory;
using Services.Input;
using Services.Raycast;
using Services.StaticData;
using UnityEngine;

namespace Ui.Services.Placing
{
    public class WindowPlacingService : IWindowPlacingService
    {
        private readonly UnityEngine.Camera _camera;
        private readonly IGameFactory _factory;
        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;
        private readonly UpdateService _updateService;
        private KeyCode _mousePlacingKey;


        public WindowPlacingService(IInputService inputService, IGameFactory factory, IRaycastService raycastService,
            IStaticDataService staticData)
        {
            _factory = factory;
            _raycastService = raycastService;
            _inputService = inputService;
            _camera = UnityEngine.Camera.main;

            InitializeControls(staticData);

            IsPlacing = false;

            _inputService.OnMouseClick += PlaceWindowIcon;
        }

        public bool IsPlacing { get; set; }

        private void InitializeControls(IStaticDataService staticData)
        {
            var controlsStaticData = staticData.ForControls();
            _mousePlacingKey = controlsStaticData.MousePlacingKey;
        }

        private void PlaceWindowIcon()
        {
            if (IsPlacing)
                if (_inputService.GetKey(_mousePlacingKey))
                {
                    var ray = _camera.ScreenPointToRay(_inputService.MousePosition());
                    Debug.DrawRay(_camera.transform.position, _inputService.MousePosition(), Color.blue, 4f);
                    if (_raycastService.PhysicsRaycast(ray, out var hit))
                    {
                        Debug.Log(hit.point);
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