namespace Ui.Services
{
    public interface IWindowPlacingService
    {
        bool IsPlacing { get; set; }
        void PlaceWindowIcon();
    }
}