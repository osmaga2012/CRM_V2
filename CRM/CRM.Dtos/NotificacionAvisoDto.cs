namespace CRM.Dtos
{
    /// <summary>
    /// Encapsula la configuración de avisos y frecuencia para un trámite específico
    /// </summary>
    public class NotificacionAvisoDto
    {
        /// <summary>
        /// Días antes del vencimiento para enviar el primer aviso
        /// </summary>
        public int DiasAntesVencimiento { get; set; } = 15;

        /// <summary>
        /// Tipo de frecuencia para recordatorios posteriores al primer aviso
        /// </summary>
        public TipoFrecuencia TipoFrecuencia { get; set; } = TipoFrecuencia.None;

        /// <summary>
        /// Solo se usa si TipoFrecuencia == Personalizado
        /// Especifica cada cuántas horas se envía un recordatorio
        /// </summary>
        public int? CadaXHoras { get; set; }

        /// <summary>
        /// Indica si los avisos están habilitados para este trámite
        /// </summary>
        public bool Activo { get; set; } = true;
    }
}