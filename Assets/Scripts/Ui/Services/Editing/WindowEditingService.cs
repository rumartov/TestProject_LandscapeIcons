using System.Collections.Generic;
using Services.Input;
using Services.Raycast;
using Services.StaticData;
using Ui.Services.Placing;
using Ui.Services.Selection;
using Ui.Window;
using Ui.Window.Infrastructure;
using UnityEngine;

namespace Ui.Services.Editing
{
    internal class WindowEditingService : IWindowEditingService
    {
        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;
        private readonly IStaticDataService _staticData;

        private readonly IWindowPlacingService _windowPlacingService;
        private readonly IWindowSelectionService _windowSelectionService;
        private readonly IWindowService _windowService;

        private IconEditMenu _currentEditingWindow;
        public KeyCode deleteButton;

        public WindowEditingService(IInputService inputService, IWindowPlacingService windowPlacingService,
            IWindowSelectionService windowSelectionService, IWindowService windowService,
            IRaycastService raycastService, IStaticDataService staticData)
        {
            _inputService = inputService;
            _windowPlacingService = windowPlacingService;
            _windowSelectionService = windowSelectionService;
            _windowService = windowService;
            _raycastService = raycastService;
            _staticData = staticData;

            _windowSelectionService.OnDeselectAll += DeleteEditingMenu;

            var controlsStaticData = _staticData.ForControls();
            deleteButton = controlsStaticData.DeleteIconsButton;

            CurrentEditingWindowIconsList = new List<WindowBase>();

            _inputService.OnMouseClick += Edit;
            _inputService.OnKeyDown += DeleteIcons;
        }

        public List<WindowBase> CurrentEditingWindowIconsList { get; set; }

        public void Edit()
        {
            if (!_windowPlacingService.IsPlacing)
                if (_inputService.LeftMouseDown())
                {
                    var hit = _raycastService.RaycastAll(_inputService.MousePosition());

                    if (!HasWindowIcon(hit)) return;

                    var windowIcon = hit.GetComponentInParent<WindowIcon>();
                    var windowId = windowIcon.WindowId;

                    CurrentEditingWindowIconsList = _windowSelectionService.SelectedWindowsList;

                    _currentEditingWindow = _windowService.Open(windowId).GetComponent<IconEditMenu>();
                }
        }

        public void DeleteSelectedWindowsIcons()
        {
            CurrentEditingWindowIconsList = _windowSelectionService.SelectedWindowsList;
            foreach (WindowIcon windowIcon in CurrentEditingWindowIconsList)
                if (windowIcon != null)
                    windowIcon.Close();

            DeleteEditingMenu();
        }

        private void DeleteIcons(KeyCode code)
        {
            if (code == deleteButton) DeleteSelectedWindowsIcons();
        }

        private void DeleteEditingMenu()
        {
            if (_currentEditingWindow != null) _currentEditingWindow.Close();
        }

        private bool HasWindowIcon(GameObject hit)
        {
            if (hit == null) return false;

            if (hit.GetComponentInParent<WindowIcon>() == null) return false;

            return true;
        }
    }
}