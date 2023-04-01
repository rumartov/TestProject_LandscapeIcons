using UnityEngine;

namespace Services
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string assetPath, Vector3 position)
        {
            return Object.Instantiate(Resources.Load<GameObject>(assetPath), position, Quaternion.identity);
        }

        public GameObject Instantiate(string assetPath, Transform parent)
        {
            return Object.Instantiate(Resources.Load<GameObject>(assetPath), parent);
        }

        public GameObject Instantiate(string assetPath)
        {
            return Object.Instantiate(Resources.Load<GameObject>(assetPath));
        }
    }
}