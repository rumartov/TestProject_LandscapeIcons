using TMPro;
using Ui.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Window
{
    public class IconEditMenu : WindowBase
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button deleteWindowIconsButton;
        private IWindowEditingService _windowEditingService;

        private IWindowPlacingService _windowPlacingService;

        public void Construct(IWindowPlacingService windowPlacingService, IWindowEditingService windowEditingService)
        {
            _windowPlacingService = windowPlacingService;
            _windowEditingService = windowEditingService;

            inputField.onSubmit.AddListener(UpdateWindowIconsText);
            deleteWindowIconsButton.onClick.AddListener(DeleteSelectedWindowsIcons);
        }

        private void DeleteSelectedWindowsIcons()
        {
            foreach (WindowIcon windowIcon in _windowEditingService.CurrentEditingWindowIconsList)
                Destroy(windowIcon.gameObject);
            Destroy(gameObject);
        }

        private void UpdateWindowIconsText(string targetText)
        {
            foreach (WindowIcon windowIcon in _windowEditingService.CurrentEditingWindowIconsList)
                windowIcon.Text = targetText;
        }
    }
}