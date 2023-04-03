using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "TerrainData", menuName = "Static Data/Terrain")]
    public sealed class TerrainStaticData : ScriptableObject
    {
        public float LegthX = 100f;
        public float WidthZ = 100f;
    }
}