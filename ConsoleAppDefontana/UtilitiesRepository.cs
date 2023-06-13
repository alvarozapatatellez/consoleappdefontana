using BusinessLogic.DataContext;
using ConsoleTables;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppDefontana
{
    public class UtilitiesRepository
    {
        private readonly ConfigDbContext dbContext;

        public UtilitiesRepository() {
            string _connectionString = "Server = lab-defontana.caporvnn6sbh.us-east-1.rds.amazonaws.com; Database = Prueba; User Id = ReadOnly; Password = d*3PSf2MmRX9vJtA5sgwSphCVQ26*T53uU; TrustServerCertificate = True;";
            // Configurar el contexto de la base de datos
            var options = new DbContextOptionsBuilder<ConfigDbContext>()
                .UseSqlServer(_connectionString)
                .Options;
            //var dbContext = new ConfigDbContext(options);
            dbContext = new ConfigDbContext(options);
        }
        
        public List<DetalleVentaDto> consultaDetalleDeVentas(int cantDiasdesde)
        {
            List<DetalleVentaDto> resultadoDto = new List<DetalleVentaDto>() ;
            try
            {
                cantDiasdesde += -1;
                var fechadesde = DateTime.Today.AddDays(cantDiasdesde *-1);
                var fechahasta = DateTime.Now.Date;
                var result = dbContext.Venta
                .Join(dbContext.VentaDetalle, v => v.ID_Venta, vd => vd.VentaId, (venta, ventadet) => new { venta, ventadet })
                .Join(dbContext.Producto, vd => vd.ventadet.ProductoId, p => p.ID_Producto, (vd, product) => new { vd.venta, vd.ventadet, product })
                .Join(dbContext.Local, v => v.venta.LocalId, l => l.ID_Local, (v, local) => new { v.venta, v.ventadet, v.product, local })
                .Join(dbContext.Marca, p => p.product.MarcaId, m => m.ID_Marca, (p, marca) => new { p.local, p.venta, p.ventadet, p.product, marca })
                .Where(data => data.venta.Fecha >= fechadesde && data.venta.Fecha <= fechahasta.AddDays(1).AddTicks(-1))
                .Select(data => new DetalleVentaDto
                {
                    IdLocal = data.local.ID_Local,
                    NombreLocal = data.local.Nombre,
                    Direccion = data.local.Direccion,
                    IdVenta = data.venta.ID_Venta,
                    Fecha = data.venta.Fecha,
                    Total = data.venta.Total,
                    IdVentaDetalle = data.ventadet.ID_VentaDetalle,
                    PrecioUnitario = data.ventadet.Precio_Unitario,
                    Cantidad = data.ventadet.Cantidad,
                    TotalLinea = data.ventadet.TotalLinea,
                    IdProducto = data.product.ID_Producto,
                    Codigo = data.product.Codigo,
                    ProductoNombre = data.product.Nombre,
                    IdMarca = data.marca.ID_Marca,
                    MarcaNombre = data.marca.Nombre,
                    Modelo = data.product.Modelo,
                    CostoUnitario = data.product.Costo_Unitario
                }).ToList();

                if (result == null || !result.Any()) throw new Exception("No hay datos para mostrar.");

                resultadoDto = result;                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultadoDto;
        }

        public ConsoleTable MostrarFacturas(List<DetalleVentaDto> detalleVentaDtos)
        {
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            var table = new ConsoleTable("IdLocal", "NombreLocal", "IdVenta", "Fecha", "Total");
            var data = detalleVentaDtos.Select(v => new { v.IdLocal, v.NombreLocal, v.IdVenta, v.Fecha, v.Total }).Distinct().OrderBy(v => v.IdLocal).ThenBy(v => v.IdVenta).ToList();
            foreach (var item in data)
            {
                table.AddRow(item.IdLocal, item.NombreLocal, item.IdVenta, item.Fecha.ToString("dd/MM/yyyy"), item.Total);
            }
            return table;
        }

        public ConsoleTable MostarTotalVentas(List<DetalleVentaDto> detalleVentaDtos)
        {
            var montoTotal = detalleVentaDtos.Sum(v => v.TotalLinea);
            var cantTotalVentas = detalleVentaDtos.Select(v => new { v.IdLocal, v.NombreLocal, v.IdVenta, v.Fecha, v.Total }).Distinct().Count();
            var table = new ConsoleTable("Monto Total", "Cant. Ventas");
            table.AddRow(montoTotal, cantTotalVentas);

            return table;
        }

        public ConsoleTable MostarProductoMayorVenta(List<DetalleVentaDto> detalleVentaDtos)
        {
            var ProductoMasVendido = detalleVentaDtos.GroupBy(v => v.IdProducto).Select(d => new
            {
                IdProducto = d.Key,
                TotalVenta = d.Sum(v => v.TotalLinea),
            }).OrderByDescending(x => x.TotalVenta).FirstOrDefault();

            var producto = detalleVentaDtos.FirstOrDefault(v => v.IdProducto == ProductoMasVendido.IdProducto);                 
            
            var table = new ConsoleTable("IdProducto", "Codigo", "ProductoNombre", "Monto");
            table.AddRow(producto.IdProducto, producto.Codigo, producto.ProductoNombre, ProductoMasVendido.TotalVenta);

            return table;
        }

        public ConsoleTable MostarLocalMayorVenta(List<DetalleVentaDto> detalleVentaDtos)
        {
            var LocalMasVendido = detalleVentaDtos.GroupBy(v => v.IdLocal).Select(d => new
            {
                IdLocal = d.Key,
                TotalVenta = d.Sum(v => v.TotalLinea),
            }).OrderByDescending(x => x.TotalVenta).FirstOrDefault();

            var local = detalleVentaDtos.FirstOrDefault(v => v.IdLocal == LocalMasVendido.IdLocal);

            var table = new ConsoleTable("IdLocal", "NombreLocal", "Monto");
            table.AddRow(local.IdLocal, local.NombreLocal, LocalMasVendido.TotalVenta);

            return table;
        }

        public ConsoleTable MostrarMarcaMayorMargen(List<DetalleVentaDto> detalleVentaDtos)
        {
            var MarcaMayorMargen = detalleVentaDtos.GroupBy(v => v.IdMarca).Select(d => new
            {
                IdMarca = d.Key,
                MargenVenta = (1-d.Sum(v => decimal.Parse(v.CostoUnitario.ToString()))/d.Sum(v => decimal.Parse(v.PrecioUnitario.ToString())))*100,
            }).OrderByDescending(x => x.MargenVenta).FirstOrDefault();

            var marca = detalleVentaDtos.FirstOrDefault(v => v.IdMarca == MarcaMayorMargen.IdMarca);

            var table = new ConsoleTable("IdMarca", "NombreMarca", "Magen");
            table.AddRow(marca.IdMarca, marca.MarcaNombre, MarcaMayorMargen.MargenVenta.ToString("N2"));

            return table;
        }

        public ConsoleTable MostarLocalProductoMasVendido(List<DetalleVentaDto> detalleVentaDtos)
        {
            var ProductoLocalMasVendido = detalleVentaDtos.GroupBy(v => v.IdLocal).Select(d => new
            {
                IdLocal = d.Key,
                ProductoMasVendido = d.GroupBy(v => v.IdProducto).Select(dv => new
                {
                    IdProducto = dv.Key,
                    CantidadVentas = dv.Sum(x => x.Cantidad)
                })
                .OrderByDescending(x => x.CantidadVentas)
                .FirstOrDefault()
            }).ToList();

            var table = new ConsoleTable("IdLocal", "NombreLocal", "IdProducto", "NombreProducto", "CantidadVentas");
            foreach(var local in ProductoLocalMasVendido.OrderBy(o => o.IdLocal))
            {

                var producto = detalleVentaDtos.FirstOrDefault(v => v.IdLocal == local.IdLocal && v.IdProducto == local.ProductoMasVendido.IdProducto);
                table.AddRow(local.IdLocal, producto.NombreLocal, producto.IdProducto, producto.ProductoNombre, local.ProductoMasVendido.CantidadVentas);
            }            

            return table;
        }
    }
}
