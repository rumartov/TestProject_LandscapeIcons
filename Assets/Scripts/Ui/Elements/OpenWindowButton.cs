using Ui.Services;
using Ui.Window.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        public Button Button;
        public WindowId WindowId;
        private IWindowService _windowService;

        private void Awake()
        {
            Button.onClick.AddListener(Open);
        }

        public void Init(IWindowService windowService)
        {
            _windowService = windowService;
        }

        private void Open()
        {
            _windowService.Open(WindowId);
        }
    }
}