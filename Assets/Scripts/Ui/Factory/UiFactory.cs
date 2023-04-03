using Services.Animation;
using Services.AssetProvider;
using Services.Input;
using Ui.Services.Editing;
using Ui.Services.Placing;
using Ui.Window;
using UnityEngine;

namespace Ui.Factory
{
    public class UiFactory : IUiFactory
    {
        private readonly IAnimationService _animationService;
        private readonly AssetProvider _assete;
        private readonly IAssetProvider _assets;
        private readonly IInputService _inputService;

        private IWindowEditingService _windowEditingService;
        private IWindowPlacingService _windowPlacingService;

        public UiFactory(AssetProvider assets, IInputService inputService, IAnimationService animationService)
        {
            _assets = assets;
            _inputService = inputService;
            _animationService = animationService;
        }

        public IconEditMenu IconEditMenu { get; set; }
        public Transform UiRoot { get; set; }

        public void CreateUiRoot()
        {
            var root = _assets.Instantiate(AssetPath.UiRoot);
            UiRoot = root.GetComponent<UiRoot>().content.transform;
        }

        public GameObject CreateIconsCreationMenu()
        {
            var iconCreationMenu = _assets.Instantiate(AssetPath.IconCreationMenu, UiRoot);
            iconCreationMenu.GetComponent<IconCreationMenu>().Construct(_animationService, _windowPlacingService);
            return iconCreationMenu;
        }

        public GameObject CreateEditIconMenu()
        {
            DestroyMenuIfExist();

            var iconEditMenu = _assets.Instantiate(AssetPath.IconEditMenu, UiRoot);

            var editMenu = iconEditMenu.GetComponent<IconEditMenu>();
            editMenu.Construct(_animationService, _windowEditingService);

            IconEditMenu = editMenu;

            return iconEditMenu;
        }

        public void InjectWindowServices(IWindowPlacingService windowPlacingService,
            IWindowEditingService windowEditingService)
        {
            _windowPlacingService = windowPlacingService;
            _windowEditingService = windowEditingService;
        }

        private void DestroyMenuIfExist()
        {
            if (IconEditMenu != null) IconEditMenu.Close();
        }
    }
}