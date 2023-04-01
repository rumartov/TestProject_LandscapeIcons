using System.Linq;
using DefaultNamespace.StaticData;
using UnityEngine;

namespace DefaultNamespace
{
    class StaticDataService : IStaticDataService
    {
        private const string CameraDataPath = "StaticData/Camera";

        private CameraStaticData _camera;

        public void Load()
        {
            _camera = Resources
                .LoadAll<CameraStaticData>(CameraDataPath)
                .First();
        }

        public CameraStaticData ForCamera()
        {
            return _camera;
        }

    }
}