using UnityEngine;

namespace Ui.Services
{
    public interface IWindowSelectionVisualService
    {
        RectTransform BoxVisual { get; set; }
        bool UnitIsInSelectionBox(Vector2 position, Bounds bounds);
    }
}