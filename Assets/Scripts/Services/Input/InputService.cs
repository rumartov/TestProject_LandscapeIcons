using System;
using UnityEngine;

namespace Services.Input
{
    internal class InputService : IInputService
    {
        public Action OnMouseClick { get; set; }
        public Action OnMouseHold { get; set; }
        public Action OnMouseUp { get; set; }

        public Action<KeyCode> OnKeyDown { get; set; }

        public bool IsMouseButtonDown()
        {
            var isMouseButtonDown = UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetMouseButtonDown(1);
            if (isMouseButtonDown) OnMouseClick?.Invoke();

            return isMouseButtonDown;
        }

        public bool IsMouseButtonHold()
        {
            var isMouseButtonHeld = UnityEngine.Input.GetMouseButton(0) || UnityEngine.Input.GetMouseButton(1);
            if (isMouseButtonHeld) OnMouseHold?.Invoke();

            return isMouseButtonHeld;
        }

        public bool IsMouseButtonUp()
        {
            var isMouseButtonUp = UnityEngine.Input.GetMouseButtonUp(0) || UnityEngine.Input.GetMouseButtonUp(1);
            if (isMouseButtonUp) OnMouseUp?.Invoke();

            return isMouseButtonUp;
        }

        public void IsKeyCodeDown()
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                if (UnityEngine.Input.GetKeyDown(keyCode))
                    OnKeyDown?.Invoke(keyCode);
        }

        public bool LeftMouseHold()
        {
            return UnityEngine.Input.GetMouseButton(0);
        }

        public bool GetKey(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKey(keyCode);
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            OnKeyDown?.Invoke(keyCode);
            return UnityEngine.Input.GetKeyDown(keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyUp(keyCode);
        }

        public bool LeftMouseDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        public Vector3 MousePosition()
        {
            return UnityEngine.Input.mousePosition;
        }

        public bool RightMouseDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(1);
        }

        public bool LeftMouseUp()
        {
            return UnityEngine.Input.GetMouseButtonUp(0);
        }

        /*public bool LeftCtrl()
        {
            return Input.GetKey(KeyCode.LeftControl);
        }*/
    }
}