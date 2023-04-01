using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "ControlsData", menuName = "Static Data/Controls")]
    public sealed class ControlsStaticData : ScriptableObject
    {
        public KeyCode DeleteIconsButton = KeyCode.Delete;
    }
}