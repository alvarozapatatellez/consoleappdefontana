using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Core.Entities
{
    public class Venta
    {
        [Key]
        public long ID_Venta { get; set; }
        public int Total { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("Local")]
        [Column("ID_Local")]
        public long LocalId { get; set; }

        public virtual Local Local { get; set; }

        public virtual ICollection<VentaDetalle> VentaDetalles { get;set; }
    }
}
