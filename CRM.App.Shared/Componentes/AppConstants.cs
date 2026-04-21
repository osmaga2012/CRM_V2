using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CRM.App.Shared.Componentes
{
    public static class AppConstants
    {
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString(3);
        }
    }
}
