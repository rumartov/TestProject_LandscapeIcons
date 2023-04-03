using System.Collections.Generic;
using Ui.Window.Infrastructure;

namespace Ui.Services.Editing
{
    public interface IWindowEditingService
    {
        List<WindowBase> CurrentEditingWindowIconsList { get; set; }
        public void Edit();
        void DeleteSelectedWindowsIcons();
    }
}