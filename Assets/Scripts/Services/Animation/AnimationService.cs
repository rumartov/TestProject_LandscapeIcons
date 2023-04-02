using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Ui.Window;
using UnityEngine;

public sealed class AnimationService : IAnimationService
{
    public TweenerCore<Vector3, Vector3, VectorOptions> ScaleUp(Transform transform, float duration)
    {
        Vector3 targetScale = transform.localScale;
        transform.localScale = new Vector3(0,0,0);
        return transform.DOScale(targetScale, duration);
    }

    public TweenerCore<Vector3, Vector3, VectorOptions> ScaleDown(Transform transform, float duration)
    {
        Vector3 targetScale = new Vector3(0,0,0);
        return transform.DOScale(targetScale, duration);
    }
}