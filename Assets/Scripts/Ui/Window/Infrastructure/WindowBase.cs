using System;
using System.Collections;
using DG.Tweening;
using Services.Animation;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Window.Infrastructure
{
    public abstract class WindowBase : MonoBehaviour, IWindowBase
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Image image;

        private IAnimationService _animationService;
        private readonly float _duration = 0.5f;

        private void Awake()
        {
            OnAwake();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        public virtual void Close()
        {
            StartCoroutine(PreDestroy(() => Destroy(gameObject)));
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

        public void Construct(IAnimationService animationService)
        {
            _animationService = animationService;
        }

        private IEnumerator PreDestroy(Action onClosed)
        {
            var completed = false;

            _animationService
                .ScaleDown(transform, _duration)
                .OnComplete(() => completed = true);

            while (!completed)
                yield return null;

            Debug.Log("Completed animation");
            onClosed.Invoke();
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