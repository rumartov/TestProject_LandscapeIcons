using System;
using UnityEngine;

namespace Services
{
    public interface IRaycastService
    {
        GameObject RaycastAll(Vector2 mousePosition);
        bool PhysicsRaycast(Ray ray, out RaycastHit hit, int layerMask);
        bool PhysicsRaycast(Ray ray, out RaycastHit hit, float maxDistance);
        bool PhysicsRaycast(Ray ray, out RaycastHit hit);
    }
}