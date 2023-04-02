using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Window
{
    public abstract class WindowBase : MonoBehaviour, IWindowBase
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Image image;
        
        private IAnimationService _animationService;
        float _duration = 0.5f;
        
        public void Construct(IAnimationService animationService)
        {
            _animationService = animationService;
        }
        public virtual void Close()
        {
            StartCoroutine(PreDestroy(() => Destroy(gameObject)));
        }

        private IEnumerator PreDestroy(Action onClosed)
        {
            bool completed = false;
            
            _animationService
                .ScaleDown(transform, _duration)
                .OnComplete(() => completed = true);
            
            while (!completed)
                yield return null;
            
            Debug.Log("Completed animation");
            onClosed.Invoke();
        }

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
            closeButton?.onClick.AddListener(Close);
        }

        protected virtual void Cleanup()
        {
        }
    }
}