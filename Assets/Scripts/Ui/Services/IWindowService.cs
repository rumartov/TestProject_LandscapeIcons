using Ui.Window.Infrastructure;
using UnityEngine;

namespace Ui.Services
{
    public interface IWindowService
    {
        GameObject Open(WindowId windowId);
    }
}