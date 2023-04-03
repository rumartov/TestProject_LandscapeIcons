using UnityEngine;

namespace Ui.Services.Selection
{
    public interface IWindowSelectionVisualService
    {
        RectTransform BoxVisual { get; }
        bool UnitIsInSelectionBox(Vector2 position);
    }
}