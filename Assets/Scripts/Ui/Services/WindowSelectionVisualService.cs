using Services;
using UnityEngine;

namespace Ui.Services
{
    internal class WindowSelectionVisualService : IWindowSelectionVisualService
    {
        private readonly IInputService _inputService;
        private readonly Camera _camera;
        private Vector3 _endPosition;

        private Vector3 _startPosition;

        public WindowSelectionVisualService(IInputService inputService,
            RectTransform rectTransform)
        {
            _inputService = inputService;

            _startPosition = Vector2.zero;
            _endPosition = Vector2.zero;
            _camera = Camera.main;

            BoxVisual = rectTransform;

            _inputService.OnMouseClick += MouseClick;
            _inputService.OnMouseHold += MouseHeld;
            _inputService.OnMouseUp += MouseUp;
        }

        public RectTransform BoxVisual { get; set; }

        public bool UnitIsInSelectionBox(Vector2 position, Bounds bounds)
        {
            return position.x > bounds.min.x && position.x < bounds.max.x
                                             && position.y > bounds.min.y && position.y < bounds.max.y;
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

        private void MouseHeld()
        {
            if (ControlsPressed())
            {
                _endPosition = _inputService.MousePosition();
                _camera.DisablePanning();
            }

            DrawVisual();
        }

        private bool ControlsPressed()
        {
            return _inputService.LeftMouseHold() && _inputService.LeftCtrl();
        }

        private void StartSelecting()
        {
            if (_startPosition == Vector3.zero)
            {
                _startPosition = _inputService.MousePosition();
                new Rect();
            }
        }

        private void DrawVisual()
        {
            Vector2 boxStart = _startPosition;
            Vector2 boxEnd = _endPosition;

            var boxCenter = (boxStart + boxEnd) / 2;
            BoxVisual.position = boxCenter;

            var boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

            BoxVisual.sizeDelta = boxSize;
        }
    }
}