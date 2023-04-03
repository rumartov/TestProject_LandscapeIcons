using System;
using System.Collections.Generic;
using Ui.Window.Infrastructure;

namespace Ui.Services.Selection
{
    public interface IWindowSelectionService
    {
        List<WindowBase> SelectedWindowsList { get; set; }
        public Action OnDeselectAll { get; set; }
        public void OnClick();
        public void Select(WindowBase window);
        public void MultipleSelect();
        public void DeselectAll();
        void Deselect(WindowBase window);
    }
}