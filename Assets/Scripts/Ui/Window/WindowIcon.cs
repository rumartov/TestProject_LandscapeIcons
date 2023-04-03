using System;
using TMPro;
using UnityEngine;

namespace Ui.Window
{
    public class WindowIcon : WindowBase, ISelectableWindow
    {
        //TODO add animation static data
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private TextMeshProUGUI tmpText;
        [SerializeField] private float maxScale = 0.5f;
        [SerializeField] private float minScale = 0.2f;

        private Vector3 _baseLocalScale;

        private string _text;
        private IAnimationService _animationService;
        private Vector3 _defaultSize;
        public Action OnTextUpdate { get; set; }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnTextUpdate?.Invoke();
            }
        }

        public int WindowIconId { get; set; }
        public WindowId WindowId { get; set; }

        private void Awake()
        {
            _baseLocalScale = transform.localScale;
        }

        public void Construct(int windowIconId, IAnimationService animationService)
        {
            base.Construct(animationService);
            
            WindowIconId = windowIconId;
            WindowId = WindowId.WindowIcon;

            Text = "New WindowIcon";
            OnTextUpdate += UpdateIcon;

            _animationService = animationService;
            
            _animationService.ScaleUp(transform, duration);
        }

        private void UpdateIcon()
        {
            float newScale = Text.Length / 2f;

            if (newScale <= 0)
                return;
            
            ReScaleTarget(newScale); 
            
            RaiseTarget(transform);

            tmpText.text = _text;
        }

        private void RaiseTarget(Transform target)
        {
            var position = target.position;
            position = new Vector3(
                transform.position.x,
                _baseLocalScale.y + position.y,
                position.z);
            target.position = position;
        }

        private void ReScaleTarget(float newScale)
        {
            if (_baseLocalScale.x * newScale > maxScale)
            {
                SetScaleToTarget(maxScale, transform);
                return;
            }

            if (_baseLocalScale.x * newScale < minScale)
            {
                SetScaleToTarget(minScale, transform);
                return;
            }
            
            SetScaleToTarget(newScale * _baseLocalScale.x, transform);
        }

        private void SetScaleToTarget(float newScale, Transform target)
        {
            float scaleX = newScale;
            float scaleY = newScale;
            float scaleZ = newScale;
            target.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            OnTextUpdate -= UpdateIcon;
        }
    }
}