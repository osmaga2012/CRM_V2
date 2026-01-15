using CRM.App.Shared.Services;

namespace CRM.App.Web.Client.Services
{
    public class FormFactor : IFormFactor
    {
        public bool IsNativeApp => false;

        public string GetFormFactor()
        {
            return "WebAssembly";
        }

        public string GetPlatform()
        {
            return Environment.OSVersion.ToString();
        }
    }
}
