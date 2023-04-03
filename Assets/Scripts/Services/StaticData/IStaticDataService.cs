using StaticData;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        void Load();
        CameraStaticData ForCamera();
        ControlsStaticData ForControls();
        TerrainStaticData ForTerrain();
    }
}