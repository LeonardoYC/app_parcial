using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace app_parcial.Models
{
    [Table("t_Historial_Conversion")]
    public class HistorialConversion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public decimal TasaUSDaBTC { get; set; } // Tasa en el momento de la conversión

        public DateTime Fecha { get; set; } = DateTime.UtcNow; // Establecer a UTC por defecto
    }
}