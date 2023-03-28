using UnityEngine;

namespace Services
{
    public sealed class UpdateService : MonoBehaviour
    {
        private IInputService _inputService;

        private void Update()
        {
            _inputService?.IsMouseButtonClicked();
            _inputService?.IsMouseButtonHeld();
            _inputService?.IsMouseButtonUp();
        }

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
    }
}