using System.Collections.Generic;
using Camera;
using RTS_Cam;
using Services.Animation;
using Services.Input;
using Services.Random;
using Services.Raycast;
using Services.StaticData;
using Ui.Elements;
using Ui.Factory;
using Ui.Services;
using Ui.Services.Selection;
using Ui.Window;
using UnityComponents;
using UnityEngine;

namespace Services.Factory
{
    internal class GameFactory : IGameFactory
    {
        private readonly IAnimationService _animationService;
        private readonly AssetProvider.AssetProvider _assets;
        private readonly IInputService _inputService;
        private readonly IRandomService _random;
        private readonly IRaycastService _raycastService;
        private readonly IStaticDataService _staticData;
        private readonly UiFactory _uiFactory;
        private readonly IWindowService _windowService;

        private IWindowSelectionService _selectionWindowService;

        public GameFactory(AssetProvider.AssetProvider assets, IWindowService windowService, UiFactory uiFactory,
            IRandomService random, IRaycastService raycastService, IInputService inputService,
            IStaticDataService staticData, IAnimationService animationService)
        {
            _assets = assets;
            _windowService = windowService;
            _uiFactory = uiFactory;
            _random = random;
            _raycastService = raycastService;
            _inputService = inputService;
            _staticData = staticData;
            _animationService = animationService;

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
            windowIconComponent.Construct(windowIconId, _animationService);

            var windowIconMovement = windowIcon.GetComponent<WindowIconMovement>();
            windowIconMovement.Construct(_raycastService);

            DestroyIconCreationMenu();

            WindowIconList = UpdateWindowIconList();

            WindowIconList.Add(windowIconComponent);

            return windowIcon;
        }

        public void CreateCamera()
        {
            var camera = UnityEngine.Camera.main;

            var cameraStaticData = _staticData.ForCamera();

            var rtsCamera = camera.GetComponent<RTS_Camera>();
            rtsCamera.panningKey = cameraStaticData.PanningKey;
            rtsCamera.panningSpeed = cameraStaticData.PanningSpeed;

            var rotateAroundTarget = camera.GetComponent<RotateAroundTarget>();
            rotateAroundTarget.distanceToTarget = cameraStaticData.DistanceToTargetOnRotation;
            rotateAroundTarget.keyboardRotationKey = cameraStaticData.KeyboardRotationKey;
            rotateAroundTarget.mouseRotationKey = cameraStaticData.MouseRotationKey;

            rotateAroundTarget.Construct(_inputService, _raycastService);
        }

        private void DestroyIconCreationMenu()
        {
            _uiFactory.UiRoot.GetComponentInChildren<IconCreationMenu>().Close();
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
    }
}