using System.Collections.Generic;
using Services;
using Ui.Window;
using UnityEngine;

namespace Ui.Services
{
    public class WindowSelectionService : IWindowSelectionService
    {
        private readonly Camera _camera;
        private readonly IGameFactory _factory;

        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;

        private readonly int _defaultLayerMask;
        private readonly IWindowSelectionVisualService _windowSelectionVisualService;
        private Vector3 _cachedMousePosition = Vector3.zero;

        private Dictionary<int, Vector3> _windowIdToMousePosition;

        public WindowSelectionService(IInputService inputService, IGameFactory factory,
            IWindowSelectionVisualService windowSelectionVisualService, IRaycastService raycastService)
        {
            _inputService = inputService;
            _factory = factory;
            _windowSelectionVisualService = windowSelectionVisualService;
            _raycastService = raycastService;

            SelectedWindowsList = new List<WindowBase>();
            _windowIdToMousePosition = new Dictionary<int, Vector3>();

            _defaultLayerMask = 1 << 0;

            _camera = Camera.main;

            _inputService.OnMouseUp += OnClickUp;
            _inputService.OnMouseClick += OnClick;
            _inputService.OnMouseHold += OnHold;
        }

        public List<WindowBase> SelectedWindowsList { get; set; }

        public void OnClick()
        {
            if (_inputService.LeftMouseDown()) WindowIconSelection();
        }

        public void Select(WindowBase window)
        {
            SelectedWindowsList.Add(window);
            window.Highlight();
        }

        public void CtrlSelect()
        {
            var bounds = new Bounds(_windowSelectionVisualService.BoxVisual.anchoredPosition,
                _windowSelectionVisualService.BoxVisual.sizeDelta);
            foreach (var windowIcon in _factory.WindowIconList)
                if (_windowSelectionVisualService.UnitIsInSelectionBox(windowIcon.transform.position, bounds))
                    Select(windowIcon);
        }

        public void DeselectAll()
        {
            SelectedWindowsList.ForEach(x => x.UnHighlight());
            SelectedWindowsList = new List<WindowBase>();
        }

        private void OnClickUp()
        {
            if (_inputService.LeftMouseUp())
            {
                _windowIdToMousePosition = new Dictionary<int, Vector3>();
                SettingMovingWindowIconsCameraOffset();

                _camera.EnablePanning();
            }
        }

        private void SettingMovingWindowIconsCameraOffset()
        {
            var ray = _camera.ScreenPointToRay(_inputService.MousePosition());
            Debug.DrawRay(_camera.transform.position, _inputService.MousePosition(), Color.blue, 4f);
            if (_raycastService.PhysicsRaycast(ray, out var raycastHit, _defaultLayerMask))
                foreach (var windowBase in SelectedWindowsList)
                {
                    var windowIcon = (WindowIcon) windowBase;

                    if (_windowIdToMousePosition.ContainsKey(windowIcon.WindowIconId))
                        continue;

                    var windowIconToMousePosition = raycastHit.point - windowBase.transform.position;

                    _windowIdToMousePosition.Add(windowIcon.WindowIconId, windowIconToMousePosition);
                }
        }

        private void WindowIconSelection()
        {
            var data = _raycastService.RaycastAll(_inputService.MousePosition());
            if (data != null)
            {
                var window = data.gameObject.GetComponentInParent<WindowIcon>();

                if (window == null)
                {
                    if (data.layer == LayerMask.NameToLayer("UI"))
                    {
                        DeselectAll();
                        return;
                    }

                    DeselectAll();
                }

                Select(window);
            }
            else
            {
                DeselectAll();
            }
        }

        public void OnHold()
        {
            if (_inputService.LeftMouseHold() && _inputService.LeftCtrl()) CtrlSelect();

            if (_inputService.LeftMouseHold() && !_inputService.LeftCtrl()) MoveSelected();
        }

        private void MoveSelected()
        {
            if (!MinimalTouchReached())
                return;

            var ray = _camera.ScreenPointToRay(_inputService.MousePosition());
            //Debug.DrawRay(_camera.transform.position, _inputService.MousePosition(), Color.blue, 4f);
            if (_raycastService.PhysicsRaycast(ray, out var raycastHit, _defaultLayerMask))
            {
                foreach (var windowBase in SelectedWindowsList)
                {
                    var windowIcon = (WindowIcon) windowBase;
                    if (!_windowIdToMousePosition.ContainsKey(windowIcon.WindowIconId))
                        continue;


                    windowBase.transform.position =
                        raycastHit.point - _windowIdToMousePosition[windowIcon.WindowIconId];

                    _camera.DisablePanning();
                }    
            }
        }

        private bool MinimalTouchReached()
        {
            var mousePosition = _inputService.MousePosition();
            if (_cachedMousePosition == Vector3.zero) _cachedMousePosition = mousePosition;

            var magnitude = _cachedMousePosition.magnitude - mousePosition.magnitude;
            if (Mathf.Abs(magnitude)
                > Config.CameraDistanceForMovingIcons && magnitude != 0)
            {
                _cachedMousePosition = mousePosition;
                return true;
            }

            return false;
        }
    }
}