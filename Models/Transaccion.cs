using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace app_parcial.Models
{
    [Table("t_remesa")]
    public class Transaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Remitente { get; set; }

        [Required]
        public string Destinatario { get; set; }

        [Required]
        public string PaisOrigen { get; set; }

        [Required]
        public string PaisDestino { get; set; }

        [Required]
        public decimal MontoEnviado { get; set; }

        public decimal TasaCambio { get; set; }  // Tasa de conversión entre USD y BTC

        [Required]
        public string MonedaOrigen { get; set; }  // "USD" o "BTC"

        public string MonedaDestino { get; set; }  // "USD" o "BTC"

        public decimal MontoFinal { get; set; }  // Monto convertido

        public DateTime Fecha { get; set; } = DateTime.UtcNow; // Establecer a UTC por defecto

        public string Estado { get; set; } // Ejemplo: "Pendiente", "Completada"
    }
}