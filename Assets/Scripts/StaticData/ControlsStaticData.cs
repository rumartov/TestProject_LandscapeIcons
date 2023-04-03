using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "ControlsData", menuName = "Static Data/Controls")]
    public sealed class ControlsStaticData : ScriptableObject
    {
        public KeyCode DeleteIconsKey = KeyCode.Delete;

        public KeyCode MouseSelectionKey = KeyCode.Mouse0;
        public KeyCode KeyboardSelectionKey = KeyCode.LeftControl;
        public KeyCode MouseEditKey = KeyCode.Mouse0;
        public KeyCode MousePlacingKey = KeyCode.Mouse0;
    }
}