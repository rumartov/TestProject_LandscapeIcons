using System.Linq;
using DefaultNamespace.StaticData;
using UnityEngine;

namespace DefaultNamespace
{
    class StaticDataService : IStaticDataService
    {
        private const string CameraDataPath = "StaticData/Camera";
        private const string ControlsDataPath = "StaticData/Controls";
        private const string TerrainDataPath = "StaticData/Terrain";

        private CameraStaticData _cameraStaticData;
        private ControlsStaticData _controlsStaticData;
        private TerrainStaticData _terrainStaticData;


        public void Load()
        {
            _cameraStaticData = Resources
                .LoadAll<CameraStaticData>(CameraDataPath)
                .First();
            
            _controlsStaticData = Resources
                .LoadAll<ControlsStaticData>(ControlsDataPath)
                .First();
            
            _terrainStaticData = Resources
                .LoadAll<TerrainStaticData>(TerrainDataPath)
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
        
        public TerrainStaticData ForTerrain()
        {
            return _terrainStaticData;
        }
    }
}