using DefaultNamespace;
using Services;
using UnityEngine;

namespace UnityComponents
{
    public sealed class WindowIconMovement : MonoBehaviour
    {
        private IRaycastService _raycastService;

        public void Update()
        {
            MoveIcon();
        }

        public void Construct(IRaycastService raycastService)
        {
            _raycastService = raycastService;
        }

        private void MoveIcon()
        {
            var maxDistance = 100f;
            var defaultOffset = 3f;
            var ray = new Ray(transform.position, Vector3.down);
            _raycastService.PhysicsRaycast(ray, out var hit, maxDistance);

            var height = transform.position.y;

            var hitDistance = hit.distance;

            var extraOffset = defaultOffset - hitDistance;

            transform.position = new Vector3(
                transform.position.x,
                height + extraOffset,
                transform.position.z);
        }
    }
}