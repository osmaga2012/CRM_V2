using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.App.Shared.Modelos
{
    public class TramiteDisplayItem
    {
        public string Nombre { get; set; }
        public int Tipo { get; set; } // Para diferenciar si es "Trámite" o "Barco"
        public string Detalles { get; set; }
        // Añade aquí lo que necesites mostrar en el label

        public string FechaFinalizacion { get; set; }
    }
}
