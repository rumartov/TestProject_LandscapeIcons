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

        private Vector3 _baseLocalScale;

        private string _text;
        private IAnimationService _animationService;
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
            var newScale = Text.Length / 10;

            if (newScale > 1)
            {
                var scaleX = _baseLocalScale.x * newScale;
                var scaleY = _baseLocalScale.y * newScale;
                var scaleZ = _baseLocalScale.z * newScale;
                transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            }

            transform.position =
                new Vector3(transform.position.x, _baseLocalScale.y + transform.position.y, transform.position.z);

            tmpText.text = _text;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            OnTextUpdate -= UpdateIcon;
        }
    }
}