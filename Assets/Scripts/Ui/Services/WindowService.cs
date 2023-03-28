using Ui.Factory;
using Ui.Window;

namespace Ui.Services
{
    public class WindowService : IWindowService
    {
        private readonly IUiFactory _uiFactory;

        public WindowService(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.None:
                    break;
                case WindowId.CreateWindowIconMenu:
                    _uiFactory.CreateIconsCreationMenu();
                    break;
                case WindowId.WindowIcon:
                    _uiFactory.CreateEditIconMenu();
                    break;
            }
        }
    }
}