using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Services.Raycast
{
    internal class RaycastService : IRaycastService
    {
        public GameObject RaycastAll(Vector2 mousePosition)
        {
            var pointerData = new PointerEventData(EventSystem.current);
            var resultsData = new List<RaycastResult>();
            pointerData.position = mousePosition;

            EventSystem.current.RaycastAll(pointerData, resultsData);

            if (resultsData.Count > 0) return resultsData[0].gameObject;

            return null;
        }

        public bool PhysicsRaycast(Ray ray, out RaycastHit hit, int layerMask)
        {
            return Physics.Raycast(ray, out hit, 100000f, layerMask);
        }

        public bool PhysicsRaycast(Ray ray, out RaycastHit hit, float maxDistance, int layerMask)
        {
            return Physics.Raycast(ray, out hit, maxDistance, layerMask);
        }

        public bool PhysicsRaycast(Ray ray, out RaycastHit hit)
        {
            return Physics.Raycast(ray, out hit);
        }

        public bool PhysicsRaycast(Ray ray, out RaycastHit hit, float maxDistance)
        {
            return Physics.Raycast(ray, out hit, maxDistance);
        }
    }
}