using CRM.App.Shared.Services;

namespace CRM.App.Web.Services
{
    public class FormFactor : IFormFactor
    {
        public bool IsNativeApp => false;
        public string GetFormFactor()
        {
            return "Web";
        }

        public string GetPlatform()
        {
            return Environment.OSVersion.ToString();
        }
    }
}
