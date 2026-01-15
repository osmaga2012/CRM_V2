using CRM.App.Shared.Services;

namespace CRM.App.Services
{
    public class FormFactor : IFormFactor
    {
        public bool IsNativeApp => true;

        public string GetFormFactor()
        {
            return DeviceInfo.Idiom.ToString();
        }

        public string GetPlatform()
        {
            return DeviceInfo.Platform.ToString() + " - " + DeviceInfo.VersionString;
        }
    }
}
