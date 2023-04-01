using System;
using UnityEngine;

namespace Services
{
    internal class InputService : IInputService
    {
        public Action OnMouseClick { get; set; }
        public Action OnMouseHold { get; set; }
        public Action OnMouseUp { get; set; }
        
        public Action<KeyCode> OnKeyDown { get; set; }

        public bool IsMouseButtonDown()
        {
            var isMouseButtonDown = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
            if (isMouseButtonDown) OnMouseClick?.Invoke();

            return isMouseButtonDown;
        }

        public bool IsMouseButtonHold()
        {
            var isMouseButtonHeld = Input.GetMouseButton(0) || Input.GetMouseButton(1);
            if (isMouseButtonHeld) OnMouseHold?.Invoke();

            return isMouseButtonHeld;
        }

        public bool IsMouseButtonUp()
        {
            var isMouseButtonUp = Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1);
            if (isMouseButtonUp) OnMouseUp?.Invoke();

            return isMouseButtonUp;
        }
        
        public void IsKeyCodeDown()
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    OnKeyDown?.Invoke(keyCode);
                }
            }
        }
        
        public bool LeftMouseHold()
        {
            return Input.GetMouseButton(0);
        }

        public bool GetKey(KeyCode keyCode)
        {
            return Input.GetKey(keyCode);
        }
        
        public bool GetKeyDown(KeyCode keyCode)
        {
            OnKeyDown?.Invoke(keyCode);
            return Input.GetKeyDown(keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            return Input.GetKeyUp(keyCode);
        }

        public bool RightMouseDown()
        {
            return Input.GetMouseButtonDown(1);
        }

        public bool LeftMouseDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        public bool LeftMouseUp()
        {
            return Input.GetMouseButtonUp(0);
        }

        public Vector3 MousePosition()
        {
            return Input.mousePosition;
        }

        public bool LeftCtrl()
        {
            return Input.GetKey(KeyCode.LeftControl);
        }
    }
}