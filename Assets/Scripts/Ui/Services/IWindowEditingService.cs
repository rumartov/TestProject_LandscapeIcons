using System.Collections.Generic;
using Ui.Window;

namespace Ui.Services
{
    public interface IWindowEditingService
    {
        List<WindowBase> CurrentEditingWindowIconsList { get; set; }
        public void Edit();
    }
}