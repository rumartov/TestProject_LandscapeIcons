﻿using UnityEngine;

namespace Services.AssetProvider
{
    public interface IAssetProvider
    {
        public GameObject Instantiate(string assetPath, Vector3 position);
        public GameObject Instantiate(string assetPath, Transform parent);
        public GameObject Instantiate(string assetPath);
    }
}