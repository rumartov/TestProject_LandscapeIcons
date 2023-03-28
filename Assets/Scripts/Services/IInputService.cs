using System;
using UnityEngine;

namespace Services
{
    public interface IInputService
    {
        public Action OnMouseClick { get; set; }
        public Action OnMouseHold { get; set; }
        public Action OnMouseUp { get; set; }
        public bool RightMouseDown();

        public bool LeftMouseDown();

        public Vector3 MousePosition();

        public bool LeftCtrl();
        bool IsMouseButtonClicked();
        bool LeftMouseUp();
        bool IsMouseButtonHeld();
        bool IsMouseButtonUp();
        bool LeftMouseHold();
    }
}