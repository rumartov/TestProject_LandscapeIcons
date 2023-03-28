using UnityEngine;

namespace Ui.Factory
{
    public interface IUiFactory
    {
        public Transform UiRoot { get; set; }
        public GameObject CreateIconsCreationMenu();
        public void CreateUiRoot();
        GameObject CreateEditIconMenu();
    }
}