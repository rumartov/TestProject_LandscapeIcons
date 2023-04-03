using UnityEngine;

namespace Ui.Factory
{
    public interface IUiFactory
    {
        public GameObject CreateIconsCreationMenu();
        GameObject CreateEditIconMenu();
    }
}