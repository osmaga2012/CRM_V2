namespace CRM.App.Shared.Services
{
    public interface IFormFactor
    {
        bool IsNativeApp { get; }
        public string GetFormFactor();
        public string GetPlatform();
    }
}
