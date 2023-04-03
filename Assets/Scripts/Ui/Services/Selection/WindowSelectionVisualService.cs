using Services.Input;
using UnityEngine;

namespace Ui.Services.Selection
{
    internal class WindowSelectionVisualService : IWindowSelectionVisualService
    {
        private readonly UnityEngine.Camera _camera;
        private readonly IInputService _inputService;
        private Vector2 _boxCenter;

        private Vector2 _endPosition;
        private Vector2 _extents;
        private Vector2 _startPosition;
        public Rect SelectionBox;


        public WindowSelectionVisualService(IInputService inputService, RectTransform rectTransform)
        {
            _inputService = inputService;

            _startPosition = Vector2.zero;
            _endPosition = Vector2.zero;
            _camera = UnityEngine.Camera.main;

            BoxVisual = rectTransform;

            _inputService.OnMouseClick += MouseClick;
            _inputService.OnMouseHold += MouseHold;
            _inputService.OnMouseUp += MouseUp;
        }

        public RectTransform BoxVisual { get; set; }

        public bool UnitIsInSelectionBox(Vector2 position, Bounds bounds)
        {
            return SelectionBox.Contains(position);
        }

        private void MouseClick()
        {
            if (ControlsPressed()) StartSelecting();
        }

        private void MouseUp()
        {
            _startPosition = Vector2.zero;
            _endPosition = Vector2.zero;
            _camera.EnablePanning();

            DrawVisual();
        }

        private void MouseHold()
        {
            if (ControlsPressed())
            {
                _endPosition = _inputService.MousePosition();
                _camera.DisablePanning();
            }

            DrawSelection();
            DrawVisual();
        }

        private bool ControlsPressed()
        {
            //TODO update controls from static data
            return _inputService.GetKey(KeyCode.Mouse0) && _inputService.GetKey(KeyCode.LeftControl);
        }

        private void StartSelecting()
        {
            if (_startPosition == Vector2.zero)
            {
                _startPosition = _inputService.MousePosition();
                SelectionBox = new Rect();
            }
        }

        private void DrawVisual()
        {
            _boxCenter = (_startPosition + _endPosition) / 2;

            BoxVisual.position = _boxCenter;

            var boxSize = new Vector2(
                Mathf.Abs(_startPosition.x - _endPosition.x),
                Mathf.Abs(_startPosition.y - _endPosition.y));

            BoxVisual.sizeDelta = boxSize;

            _extents = boxSize / 2.0f;
        }

        private void DrawSelection()
        {
            SelectionBox.min = _boxCenter - _extents;
            SelectionBox.max = _boxCenter + _extents;
        }
    }
}