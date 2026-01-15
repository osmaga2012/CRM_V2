
namespace CRM.Web.Shared.Helpers.Traducciones
{
    public class MudDgLocalizer : MudBlazor.MudLocalizer
    {

        private static readonly Dictionary<string, string> _translations = new Dictionary<string, string>
        {
            // Traduce la clave "MudDataGrid.FilterValue" a "Buscar..."
            { "MudDataGrid.FilterValue", "Buscar..." },
            // Puedes añadir más traducciones aquí según las necesites
            //{ "MudDataGrid.GroupingText", "Arrastra aquí el encabezado de la columna para agrupar" },
            //{ "MudDataGrid.Filter", "Filtro" },
            //{ "MudDataGrid.Sort", "Ordenar" },
            //{ "MudDataGrid.ColumnMenu.Text", "Opciones de Columna" },
            //{ "MudDataGrid.Unsort", "No ordenar" },
            //{ "MudDataGrid.Unfilter", "Quitar filtro" },
        };
        public string this[string key]
        {
            get
            {
                if (_translations.ContainsKey(key))
                {
                    return _translations[key];
                }
                return key; // Devuelve la clave original si no se encuentra la traducción
            }
        }
    }
}
