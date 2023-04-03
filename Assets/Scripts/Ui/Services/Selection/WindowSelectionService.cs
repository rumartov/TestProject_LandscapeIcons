using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using Services;
using Ui.Window;
using Unity.VisualScripting;
using UnityEngine;

namespace Ui.Services
{
    public class WindowSelectionService : IWindowSelectionService
    {
        private readonly Camera _camera;
        private readonly IGameFactory _factory;

        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;
        private readonly IStaticDataService _staticData;
        
        private readonly TerrainStaticData _terrain;
        private const int DistanceFromBounds = 3;

        private readonly int _defaultLayerMask;
        private readonly IWindowSelectionVisualService _windowSelectionVisualService;

        private Dictionary<int, Vector3> _windowIdToMousePosition;
        public List<WindowBase> SelectedWindowsList { get; set; }
        public Action OnDeselectAll { get; set; }

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


            _camera = Camera.main;

            _inputService.OnMouseClick += OnClick;
            _inputService.OnMouseHold += OnHold;
        }

        public void OnClick()
        {
            if (_inputService.LeftMouseDown())
            {
                WindowIconSelection();
            
                _windowIdToMousePosition = new Dictionary<int, Vector3>();
                AddWindowIconsClickPosition();

                _camera.EnablePanning();    
            }
        }

        public void Select(WindowBase window)
        {
            SelectedWindowsList.Add(window);
            window.Highlight();
        }
        
        public void Deselect(WindowBase window)
        {
            SelectedWindowsList.Remove(window);
            window.UnHighlight();
        }

        public void CtrlSelect()
        {
            var bounds = new Bounds(_windowSelectionVisualService.BoxVisual.anchoredPosition,
                _windowSelectionVisualService.BoxVisual.sizeDelta);

            foreach (var windowIcon in _factory.WindowIconList)
            {
                Vector3 iconPosition = windowIcon.transform.position;
                
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
            Vector3 mousePos = _inputService.MousePosition();
            mousePos.z = 100f;
            mousePos = _camera.ScreenToWorldPoint(mousePos);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(_inputService.MousePosition());

                if (Physics.Raycast(ray, out var raycastHit, 100))
                {
                    Debug.DrawRay(_camera.transform.position, mousePos - _camera.transform.position,
                        Color.yellow, 2);
                   
                    foreach (var windowBase in SelectedWindowsList)
                    {
                        var windowIcon = (WindowIcon) windowBase;

                        if (!InTerrainBounds(windowIcon.transform.position) && !InTerrainBounds(raycastHit.point))
                        {
                            break;
                        }
                        
                        if (_windowIdToMousePosition.ContainsKey(windowIcon.WindowIconId))
                            continue;

                        var windowIconToMousePosition = raycastHit.point - windowBase.transform.position;
                        
                        _windowIdToMousePosition.Add(windowIcon.WindowIconId, windowIconToMousePosition);
                    }
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
            var data = _raycastService.RaycastAll(_inputService.MousePosition());
            if (data != null)
            {
                var window = data.gameObject.GetComponentInParent<WindowIcon>();

                if (window == null)
                {
                    if (data.layer == LayerMask.NameToLayer("UI"))
                    {
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
            Debug.Log("MoveSelected");

            Vector3 mousePos = _inputService.MousePosition();
            mousePos.z = 100f;
            mousePos = _camera.ScreenToWorldPoint(mousePos);
            
            var ray = _camera.ScreenPointToRay(_inputService.MousePosition());
            if (_raycastService.PhysicsRaycast(ray, out var raycastHit, _defaultLayerMask))
            {
                foreach (var windowBase in SelectedWindowsList)
                {
                    Debug.DrawRay(_camera.transform.position, mousePos - _camera.transform.position,
                        Color.magenta, 2);
                    
                    var windowIcon = (WindowIcon) windowBase;

                    if (!_windowIdToMousePosition.ContainsKey(windowIcon.WindowIconId))
                        break;

                    Vector3 newPosition = raycastHit.point - _windowIdToMousePosition[windowIcon.WindowIconId];
                    
                    if (!InTerrainBounds(newPosition))
                    {
                        break;
                    }  
                    
                    windowIcon.transform.position = newPosition;

                    _camera.DisablePanning();
                }    
            }
        }
    }
}