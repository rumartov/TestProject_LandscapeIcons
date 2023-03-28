using UnityEngine;
using UnityEngine.UI;

namespace Ui.Window
{
    public abstract class WindowBase : MonoBehaviour, IWindowBase
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Image image;

        private void Awake()
        {
            OnAwake();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        public void Highlight()
        {
            if (image == null)
                return;
            Debug.Log("Highlight");
            image.color = Color.yellow;
        }

        public void UnHighlight()
        {
            if (image == null)
                return;
            Debug.Log("UnHighlight");
            image.color = Color.white;
        }

        private void OnAwake()
        {
            closeButton?.onClick.AddListener(() => Destroy(gameObject));
        }

        protected virtual void Cleanup()
        {
        }
    }
}