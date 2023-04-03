using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Services.Animation
{
    public sealed class AnimationService : IAnimationService
    {
        public TweenerCore<Vector3, Vector3, VectorOptions> ScaleUp(Transform transform, float duration)
        {
            var targetScale = transform.localScale;
            transform.localScale = new Vector3(0, 0, 0);
            return transform.DOScale(targetScale, duration);
        }

        public TweenerCore<Vector3, Vector3, VectorOptions> ScaleDown(Transform transform, float duration)
        {
            var targetScale = new Vector3(0, 0, 0);
            return transform.DOScale(targetScale, duration);
        }
    }
}