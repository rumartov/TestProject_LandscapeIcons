using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Ui.Window
{
    public interface IAnimationService
    {
        TweenerCore<Vector3, Vector3, VectorOptions> ScaleUp(Transform transform, float duration);
        TweenerCore<Vector3, Vector3, VectorOptions> ScaleDown(Transform transform, float duration);
    }
}