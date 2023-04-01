using DefaultNamespace.StaticData;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IStaticDataService
    {
        void Load();
        CameraStaticData ForCamera();
        ControlsStaticData ForControls();
    }
}