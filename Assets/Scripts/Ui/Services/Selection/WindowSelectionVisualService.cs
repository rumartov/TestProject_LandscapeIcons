using Services.Input;
using Services.StaticData;
using UnityEngine;

namespace Ui.Services.Selection
{
    internal class WindowSelectionVisualService : IWindowSelectionVisualService
    {
        private readonly UnityEngine.Camera _camera;
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticData;

        private Vector2 _boxCenter;
        private Vector2 _endPosition;
        private Vector2 _extents;
        private KeyCode _keyboardSelectionKey;
        private KeyCode _mouseSelectionKey;

        private Rect _selectionBox;
        private Vector2 _startPosition;

        public WindowSelectionVisualService(IInputService inputService, RectTransform rectTransform,
            IStaticDataService staticData)
        {
            _inputService = inputService;
            _staticData = staticData;

            InitializeControls();

            _startPosition = Vector2.zero;
            _endPosition = Vector2.zero;
            _camera = UnityEngine.Camera.main;

            BoxVisual = rectTransform;

            _inputService.OnMouseClick += MouseClick;
            _inputService.OnMouseHold += MouseHold;
            _inputService.OnMouseUp += MouseUp;
        }

        public RectTransform BoxVisual { get; }

        public bool UnitIsInSelectionBox(Vector2 position)
        {
            return _selectionBox.Contains(position);
        }

        private void InitializeControls()
        {
            var controlsStaticData = _staticData.ForControls();
            _mouseSelectionKey = controlsStaticData.MouseSelectionKey;
            _keyboardSelectionKey = controlsStaticData.KeyboardSelectionKey;
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
            return _inputService.GetKey(_mouseSelectionKey)
                   && _inputService.GetKey(_keyboardSelectionKey);
        }

        private void StartSelecting()
        {
            if (_startPosition == Vector2.zero)
            {
                _startPosition = _inputService.MousePosition();
                _selectionBox = new Rect();
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
            _selectionBox.min = _boxCenter - _extents;
            _selectionBox.max = _boxCenter + _extents;
        }
    }
}