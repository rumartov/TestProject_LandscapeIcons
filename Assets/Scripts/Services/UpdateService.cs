using UnityEngine;

namespace Services
{
    public sealed class UpdateService : MonoBehaviour
    {
        private IInputService _inputService;

        private void Update()
        {
            _inputService?.IsMouseButtonDown();
            _inputService?.IsMouseButtonHold();
            _inputService?.IsMouseButtonUp();
            _inputService?.IsKeyCodeDown();
        }

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
    }
}