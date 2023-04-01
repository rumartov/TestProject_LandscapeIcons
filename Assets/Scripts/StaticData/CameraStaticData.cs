using UnityEngine;

namespace DefaultNamespace.StaticData
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Static Data/Camera")]
    public sealed class CameraStaticData : ScriptableObject
    {
        [Range(1, 100)] public float DistanceToTargetOnRotation = 10f;
        
        public KeyCode PanningKey = KeyCode.Mouse1;
        [Range(1, 100)] public float PanningSpeed = 25f;
        
    }
}