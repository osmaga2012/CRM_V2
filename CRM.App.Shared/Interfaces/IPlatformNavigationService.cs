namespace CRM.App.Shared.Interfaces
{
    public interface IPlatformNavigationService
    {
        void NavigateToNativePage(string route);
        Task NavigateToAsync(string route);
    }
}
