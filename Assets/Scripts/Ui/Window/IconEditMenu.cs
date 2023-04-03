using System.Linq;
using Services.Animation;
using Services.StaticData;
using TMPro;
using Ui.Services.Editing;
using Ui.Window.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Window
{
    public class IconEditMenu : WindowBase
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button deleteWindowIconsButton;
        [SerializeField] private Button submitTextButton;

        private IStaticDataService _staticData;

        private IWindowEditingService _windowEditingService;

        public void Construct(IAnimationService animationService, IWindowEditingService windowEditingService)
        {
            base.Construct(animationService);

            _windowEditingService = windowEditingService;

            var windowIcon = windowEditingService.CurrentEditingWindowIconsList
                .First()
                .GetComponent<WindowIcon>();

            inputField.text = windowIcon.Text;

            deleteWindowIconsButton.onClick.AddListener(DeleteSelectedWindowsIcons);
            submitTextButton.onClick.AddListener(() => UpdateWindowIconsText(inputField.text));
        }

        private void DeleteSelectedWindowsIcons()
        {
            _windowEditingService.DeleteSelectedWindowsIcons();
        }

        private void UpdateWindowIconsText(string targetText)
        {
            foreach (WindowIcon windowIcon in _windowEditingService.CurrentEditingWindowIconsList)
                windowIcon.Text = targetText;
        }
    }
}