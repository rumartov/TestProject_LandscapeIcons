using System.Linq;
using DefaultNamespace.StaticData;
using UnityEngine;

namespace DefaultNamespace
{
    class StaticDataService : IStaticDataService
    {
        private const string CameraDataPath = "StaticData/Camera";
        private const string ControlsDataPath = "StaticData/Controls";

        private CameraStaticData _cameraStaticData;
        private ControlsStaticData _controlsStaticData;

        public void Load()
        {
            _cameraStaticData = Resources
                .LoadAll<CameraStaticData>(CameraDataPath)
                .First();
            
            _controlsStaticData = Resources
                .LoadAll<ControlsStaticData>(ControlsDataPath)
                .First();
        }

        public CameraStaticData ForCamera()
        {
            return _cameraStaticData;
        }

        public ControlsStaticData ForControls()
        {
            return _controlsStaticData;
        }
    }
}