using DefaultNamespace;
using DG.Tweening;
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

        private IStaticDataService _staticData;
        
        public void Construct(IAnimationService animationService, IWindowEditingService windowEditingService)
        {
            base.Construct(animationService);
            
            _windowEditingService = windowEditingService;

            inputField.onSubmit.AddListener(UpdateWindowIconsText);
            deleteWindowIconsButton.onClick.AddListener(DeleteSelectedWindowsIcons);
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