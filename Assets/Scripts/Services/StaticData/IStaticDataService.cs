using DefaultNamespace.StaticData;

namespace DefaultNamespace
{
    public interface IStaticDataService
    {
        void Load();
        CameraStaticData ForCamera();
    }
}