using Ui.Window;
using UnityEngine;

namespace Ui.Services
{
    public interface IWindowService
    {
        GameObject Open(WindowId windowId);
    }
}