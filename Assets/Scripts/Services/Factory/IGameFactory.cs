using System.Collections.Generic;
using Ui.Window;
using UnityEngine;

namespace Services
{
    public interface IGameFactory
    {
        List<WindowIcon> WindowIconList { get; set; }
        GameObject CreateHud();
        GameObject CreateWindowIcon(Vector3 position);
        void CreateCamera();
    }
}