using System.Collections.Generic;
using Ui.Window;

namespace Ui.Services
{
    public interface IWindowSelectionService
    {
        List<WindowBase> SelectedWindowsList { get; set; }
        public void OnClick();
        public void Select(WindowBase window);
        public void CtrlSelect();
        public void DeselectAll();
    }
}