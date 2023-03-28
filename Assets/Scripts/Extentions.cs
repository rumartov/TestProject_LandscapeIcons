using System;
using System.Collections.Generic;
using System.Linq;
using RTS_Cam;
using UnityEngine;

public static class Extensions
{
    public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T) item.Clone()).ToList();
    }

    public static void EnablePanning(this Camera camera)
    {
        camera.GetComponent<RTS_Camera>().usePanning = true;
    }

    public static void DisablePanning(this Camera camera)
    {
        camera.GetComponent<RTS_Camera>().usePanning = false;
    }
}