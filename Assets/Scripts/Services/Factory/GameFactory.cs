using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.StaticData;
using RTS_Cam;
using Ui.Elemets;
using Ui.Factory;
using Ui.Services;
using Ui.Window;
using UnityComponents;
using UnityEngine;

namespace Services
{
    internal class GameFactory : IGameFactory
    {
        private readonly AssetProvider _assets;
        private readonly IInputService _inputService;
        private readonly IRandomService _random;
        private readonly IRaycastService _raycastService;
        private readonly UiFactory _uiFactory;
        private readonly IWindowService _windowService;
        private readonly IStaticDataService _staticData;
        
        private IWindowSelectionService _selectionWindowService;

        public GameFactory(AssetProvider assets, IWindowService windowService, UiFactory uiFactory,
            IRandomService random, IRaycastService raycastService, IInputService inputService,
            IStaticDataService staticData)
        {
            _assets = assets;
            _windowService = windowService;
            _uiFactory = uiFactory;
            _random = random;
            _raycastService = raycastService;
            _inputService = inputService;
            _staticData = staticData;

            WindowIconList = new List<WindowIcon>();
        }

        public List<WindowIcon> WindowIconList { get; set; }

        public GameObject CreateHud()
        {
            var hud = _assets.Instantiate(AssetPath.Hud);

            foreach (var openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Init(_windowService);

            return hud;
        }

        public GameObject CreateWindowIcon(Vector3 position)
        {
            var windowIcon = _assets.Instantiate(AssetPath.WindowIcon, position);

            var windowIconId = (int) _random.Range(0, Config.MaxWindowIconIdRange);

            var windowIconComponent = windowIcon.GetComponent<WindowIcon>();
            windowIconComponent.Construct(windowIconId);

            var windowIconMovement = windowIcon.GetComponent<WindowIconMovement>();
            windowIconMovement.Construct(_raycastService, _inputService);

            DestroyIconCreationMenu();

            WindowIconList = UpdateWindowIconList();

            WindowIconList.Add(windowIconComponent);

            return windowIcon;
        }

        private void DestroyIconCreationMenu()
        {
            Object.Destroy(_uiFactory.UiRoot.GetComponentInChildren<IconCreationMenu>().gameObject);
        }

        private List<WindowIcon> UpdateWindowIconList()
        {
            var list = new List<WindowIcon>();
            foreach (var icon in WindowIconList)
            {
                if (icon == null) continue;

                list.Add(icon);
            }

            return list;
        }
        
        public void CreateCamera()
        {
            Camera camera = Camera.main;

            CameraStaticData cameraStaticData = _staticData.ForCamera();
            
            RTS_Camera rtsCamera = camera.GetComponent<RTS_Camera>();
            rtsCamera.panningKey = cameraStaticData.PanningKey;
            rtsCamera.panningSpeed = cameraStaticData.PanningSpeed;
            
            RotateAroundTarget rotateAroundTarget = camera.GetComponent<RotateAroundTarget>();
            rotateAroundTarget.distanceToTarget = cameraStaticData.DistanceToTargetOnRotation;
            rotateAroundTarget.Construct(_inputService, _raycastService);
        }
    }
}