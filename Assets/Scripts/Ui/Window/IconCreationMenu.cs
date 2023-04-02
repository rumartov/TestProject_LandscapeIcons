﻿using System.Collections;
using Ui.Services;

namespace Ui.Window
{
    public sealed class IconCreationMenu : WindowBase
    {
        private IWindowPlacingService _windowPlacingService;

        public void Construct(IAnimationService animationService, IWindowPlacingService windowPlacingService)
        {
            base.Construct(animationService);
            
            _windowPlacingService = windowPlacingService;
            _windowPlacingService.IsPlacing = true;
        }
        
        protected override void Cleanup()
        {
            base.Cleanup();

            _windowPlacingService.IsPlacing = false;
        }
    }
}