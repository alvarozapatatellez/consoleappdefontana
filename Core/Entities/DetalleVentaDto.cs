using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DetalleVentaDto
    {
        public long IdLocal { get; set; }
        public string NombreLocal { get; set; }
        public string Direccion { get; set; }
        public long IdVenta { get; set; }
        public DateTime Fecha {get; set;}
        public int Total { get; set; }
        public long IdVentaDetalle {get; set;}
        public int PrecioUnitario {get; set;}
        public int Cantidad {get; set;}
        public int TotalLinea {get; set;}
        public long IdProducto {get; set;}
        public string Codigo { get; set;}
        public string ProductoNombre { get; set;}
        public long IdMarca { get; set;}
        public string MarcaNombre { get; set;}
        public string Modelo { get; set;}
        public int CostoUnitario { get; set;}
    }
}
