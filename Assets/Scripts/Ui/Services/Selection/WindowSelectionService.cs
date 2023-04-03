using System;
using System.Collections.Generic;
using Services.Factory;
using Services.Input;
using Services.Raycast;
using Services.StaticData;
using StaticData;
using Ui.Window;
using Ui.Window.Infrastructure;
using UnityEngine;

namespace Ui.Services.Selection
{
    public class WindowSelectionService : IWindowSelectionService
    {
        private const int DistanceFromBounds = 3;
        private readonly UnityEngine.Camera _camera;

        private readonly int _defaultLayerMask;
        private readonly IGameFactory _factory;

        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;
        private readonly IStaticDataService _staticData;

        private readonly TerrainStaticData _terrain;
        private readonly IWindowSelectionVisualService _windowSelectionVisualService;

        private Dictionary<int, Vector3> _windowIdToMousePosition;
        private bool _isMovingSelected;

        public WindowSelectionService(IInputService inputService, IGameFactory factory,
            IWindowSelectionVisualService windowSelectionVisualService, IRaycastService raycastService,
            IStaticDataService staticData)
        {
            _inputService = inputService;
            _factory = factory;
            _windowSelectionVisualService = windowSelectionVisualService;
            _raycastService = raycastService;
            _staticData = staticData;

            _terrain = _staticData.ForTerrain();

            SelectedWindowsList = new List<WindowBase>();
            _windowIdToMousePosition = new Dictionary<int, Vector3>();

            _defaultLayerMask = LayerMask.NameToLayer("MovableWindow");

            _camera = UnityEngine.Camera.main;
            _isMovingSelected = false;
            
            _inputService.OnMouseClick += OnClick;
            _inputService.OnMouseHold += OnHold;
        }

        public List<WindowBase> SelectedWindowsList { get; set; }
        public Action OnDeselectAll { get; set; }

        public void OnClick()
        {
            if (_inputService.GetKeyDown(KeyCode.Mouse0))
            {
                WindowIconSelection();

                _windowIdToMousePosition = new Dictionary<int, Vector3>();

                AddWindowIconsClickPosition();

                _camera.EnablePanning();
            }
        }

        public void Select(WindowBase window)
        {
            if (SelectedWindowsList.Contains(window))
                return;

            if (SelectedWindowsList.Count > 0 && !_isMovingSelected)
            {
                DeselectAll();
            }
            
            SelectedWindowsList.Add(window);

            window.Highlight();
        }

        public void Deselect(WindowBase window)
        {
            var selectedWindowsList = UpdateList(window);

            SelectedWindowsList = selectedWindowsList;

            window.UnHighlight();
        }

        private List<WindowBase> UpdateList(WindowBase window)
        {
            var selectedWindowsList = new List<WindowBase>();
            foreach (var windowBase in SelectedWindowsList)
            {
                if (windowBase == window) continue;
                selectedWindowsList.Add(windowBase);
            }

            return selectedWindowsList;
        }

        public void MultipleSelect()
        {
            var bounds = new Bounds(_windowSelectionVisualService.BoxVisual.anchoredPosition,
                _windowSelectionVisualService.BoxVisual.sizeDelta);

            foreach (var windowIcon in _factory.WindowIconList)
            {
                var iconPosition = windowIcon.transform.position;

                Vector2 worldToScreenPoint = _camera.WorldToScreenPoint(iconPosition);

                Debug.DrawRay(iconPosition, worldToScreenPoint);

                if (_windowSelectionVisualService.UnitIsInSelectionBox(worldToScreenPoint, bounds))
                    Select(windowIcon);
                else
                    Deselect(windowIcon);
            }
        }

        public void DeselectAll()
        {
            SelectedWindowsList.ForEach(x => x.UnHighlight());
            SelectedWindowsList = new List<WindowBase>();
            OnDeselectAll?.Invoke();
        }

        private void AddWindowIconsClickPosition()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(_inputService.MousePosition());

                if (Physics.Raycast(ray, out var raycastHit, 100, _defaultLayerMask))

                    foreach (var windowBase in SelectedWindowsList)
                    {
                        var windowIcon = (WindowIcon) windowBase;

                        if (_windowIdToMousePosition.ContainsKey(windowIcon.WindowIconId))
                            continue;

                        if (!InTerrainBounds(windowIcon.transform.position) &&
                            !InTerrainBounds(raycastHit.point)) break;

                        var windowIconToMousePosition = raycastHit.point - windowBase.transform.position;

                        _windowIdToMousePosition.Add(windowIcon.WindowIconId, windowIconToMousePosition);
                    }
            }
        }

        private bool InTerrainBounds(Vector3 targetPosition)
        {
            return _terrain.LegthX - DistanceFromBounds >= targetPosition.x
                   && _terrain.WidthZ - DistanceFromBounds >= targetPosition.z
                   && 0 + DistanceFromBounds <= targetPosition.x
                   && 0 + DistanceFromBounds <= targetPosition.z;
        }

        private void WindowIconSelection()
        {
            var hit = _raycastService.RaycastAll(_inputService.MousePosition());
            if (hit != null)
            {
                if (UiClicked(hit))
                    return;

                var window = hit.gameObject.GetComponentInParent<WindowIcon>();
                if (window == null) DeselectAll();

                Select(window);
            }
            else
            {
                DeselectAll();
            }
        }

        private bool UiClicked(GameObject data)
        {
            return data.layer == LayerMask.NameToLayer("UI");
        }

        public void OnHold()
        {
            if (IsSelectingMultipleIcons())
            {
                _isMovingSelected = true;
                MultipleSelect();
            }
            
            if (!IsSelectingMultipleIcons())
            {
                _isMovingSelected = false;
            }

            if (IsMovingSelected()) MoveSelected();
        }

        private bool IsMovingSelected()
        {
            return _inputService.GetKey(KeyCode.Mouse0) && !_inputService.GetKey(KeyCode.LeftControl);
        }

        private bool IsSelectingMultipleIcons()
        {
            return _inputService.GetKey(KeyCode.Mouse0) && _inputService.GetKey(KeyCode.LeftControl);
        }

        private void MoveSelected()
        {
            var ray = _camera.ScreenPointToRay(_inputService.MousePosition());
            if (_raycastService.PhysicsRaycast(ray, out var raycastHit, _defaultLayerMask))
                foreach (var windowBase in SelectedWindowsList)
                {
                    //Debug.DrawRay(_camera.transform.position, mousePos - _camera.transform.position, Color.magenta, 2);

                    var windowIcon = (WindowIcon) windowBase;

                    if (!_windowIdToMousePosition.ContainsKey(windowIcon.WindowIconId))
                        continue;

                    var newPosition = raycastHit.point - _windowIdToMousePosition[windowIcon.WindowIconId];

                    if (!InTerrainBounds(newPosition)) break;

                    windowIcon.transform.position = newPosition;

                    _camera.DisablePanning();
                }
        }
    }
}