using System;

namespace Ui.Window
{
    public interface IWindowBase
    {
        public void Highlight();
        public void UnHighlight();
        public void Close();
    }
}