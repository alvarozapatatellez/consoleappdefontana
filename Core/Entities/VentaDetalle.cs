using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class VentaDetalle
    {
        [Key]
        public long ID_VentaDetalle { get; set; }
        
        [ForeignKey("Venta")]
        [Column("ID_Venta")]
        public long VentaId { get; set; }
        public int Precio_Unitario { get; set; }
        public int Cantidad { get; set; }
        public int TotalLinea { get; set; }

        [ForeignKey("Producto")]
        [Column("ID_Producto")]
        public long ProductoId { get; set; }
    }
}
