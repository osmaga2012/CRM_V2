namespace CRM.Dtos
{
    /// <summary>
    /// Define las estrategias de frecuencia para envío de avisos recurrentes
    /// </summary>
    public enum TipoFrecuencia
    {
        /// <summary>Solo el aviso inicial a DiasAntesVencimiento, sin repeticiones</summary>
        None = 0,

        /// <summary>1 vez al día</summary>
        Diario = 1,

        /// <summary>Cada 6 horas (4 veces al día)</summary>
        Cada6Horas = 2,

        /// <summary>Cada 3 horas (8 veces al día)</summary>
        Cada3Horas = 3,

        /// <summary>Cada hora (24 veces al día)</summary>
        Cada1Hora = 4,

        /// <summary>Personalizado: Cada X horas (configurable en CadaXHoras)</summary>
        Personalizado = 5
    }
}