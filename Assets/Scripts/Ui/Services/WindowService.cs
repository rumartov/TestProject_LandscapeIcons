using Ui.Factory;
using Ui.Window.Infrastructure;
using UnityEngine;

namespace Ui.Services
{
    public class WindowService : IWindowService
    {
        private readonly IUiFactory _uiFactory;

        public WindowService(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public GameObject Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.None:
                    break;
                case WindowId.CreateWindowIconMenu:
                    return _uiFactory.CreateIconsCreationMenu();
                case WindowId.WindowIcon:
                    return _uiFactory.CreateEditIconMenu();
            }

            return null;
        }
    }
}