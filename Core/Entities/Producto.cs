using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Producto
    {
        [Key]
        public long ID_Producto { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }

        [ForeignKey("Marca")]
        [Column("ID_Marca")]
        public long MarcaId { get; set; }
        public string Modelo { get; set; }
        public int Costo_Unitario { get; set; }

        public virtual Marca Marca { get; set; }

        public virtual ICollection<VentaDetalle> VentaDetalles { get; set; }
    }
}
