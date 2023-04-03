namespace Ui.Services.Placing
{
    public interface IWindowPlacingService
    {
        bool IsPlacing { get; set; }
        void PlaceWindowIcon();
    }
}