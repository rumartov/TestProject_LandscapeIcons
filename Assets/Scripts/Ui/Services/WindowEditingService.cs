using System.Collections.Generic;
using DefaultNamespace;
using Services;
using Ui.Window;
using UnityEngine;

namespace Ui.Services
{
    internal class WindowEditingService : IWindowEditingService
    {
        public KeyCode deleteButton;
        public List<WindowBase> CurrentEditingWindowIconsList { get; set; }

        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;
        private readonly IStaticDataService _staticData;

        private readonly IWindowPlacingService _windowPlacingService;
        private readonly IWindowSelectionService _windowSelectionService;
        private readonly IWindowService _windowService;

        private GameObject _currentEditingWindow;

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
            
            ControlsStaticData controlsStaticData = _staticData.ForControls();
            deleteButton = controlsStaticData.DeleteIconsButton;
            
            CurrentEditingWindowIconsList = new List<WindowBase>();

            _inputService.OnMouseClick += Edit;
            
            _inputService.OnKeyDown += DeleteIcons; 
        }

        private void DeleteIcons(KeyCode code)
        {
            if (code == deleteButton)
            {
                DeleteSelectedWindowsIcons();
            }
        }

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

                    _currentEditingWindow = _windowService.Open(windowId);
                }
        }

        public void DeleteSelectedWindowsIcons()
        {
            foreach (WindowIcon windowIcon in CurrentEditingWindowIconsList)
                Object.Destroy(windowIcon.gameObject);
            DeleteEditingMenu();
        }

        private void DeleteEditingMenu()
        {
            Object.Destroy(_currentEditingWindow);
        }

        private bool HasWindowIcon(GameObject hit)
        {
            if (hit == null) return false;

            if (hit.GetComponentInParent<WindowIcon>() == null) return false;

            return true;
        }
    }
}