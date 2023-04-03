using System;
using System.Collections.Generic;
using Ui.Window.Infrastructure;

namespace Ui.Services.Selection
{
    public interface IWindowSelectionService
    {
        List<WindowBase> SelectedWindowsList { get; }
        public Action OnDeselectAll { get; set; }
    }
}