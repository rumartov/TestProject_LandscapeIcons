using System.Collections.Generic;
using Services;
using Ui.Window;
using UnityEngine;

namespace Ui.Services
{
    internal class WindowEditingService : IWindowEditingService
    {
        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;

        private readonly IWindowPlacingService _windowPlacingService;
        private readonly IWindowSelectionService _windowSelectionService;
        private readonly IWindowService _windowService;

        public WindowEditingService(IInputService inputService, IWindowPlacingService windowPlacingService,
            IWindowSelectionService windowSelectionService,
            IWindowService windowService, IRaycastService raycastService)
        {
            _inputService = inputService;
            _windowPlacingService = windowPlacingService;
            _windowSelectionService = windowSelectionService;
            _windowService = windowService;
            _raycastService = raycastService;

            CurrentEditingWindowIconsList = new List<WindowBase>();

            _inputService.OnMouseClick += Edit;
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

                    _windowService.Open(windowId);
                }
        }

        private bool HasWindowIcon(GameObject hit)
        {
            if (hit == null) return false;

            if (hit.GetComponentInParent<WindowIcon>() == null) return false;

            return true;
        }
    }
}