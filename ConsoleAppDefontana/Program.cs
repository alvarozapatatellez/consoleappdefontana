// See https://aka.ms/new-console-template for more information

using ConsoleAppDefontana;
using ConsoleTables;
using Core.Entities;

UtilitiesRepository utilitiesRepository = new UtilitiesRepository();


List<DetalleVentaDto> detalleVentaDtos = utilitiesRepository.consultaDetalleDeVentas(30);
while (true)
{
    Console.WriteLine("Menú de opciones:");
    Console.WriteLine("1. Cargar ventas - Se solicitara cuantos días desea analizar");
    Console.WriteLine("2. El total de ventas de los últimos días que solicito analizar en el menu 1. (monto total y cantidad total de ventas)");
    Console.WriteLine("3. Indicar cuál es el producto con mayor monto total de ventas");
    Console.WriteLine("4. Indicar el local con mayor monto de ventas");
    Console.WriteLine("5. ¿Cuál es la marca con mayor margen de ganancias?");
    Console.WriteLine("6. ¿Cómo obtendrías cuál es el producto que más se vende en cada local?");
    Console.WriteLine("7. Salir");
    Console.Write("Selecciona una opción: ");

    string opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            int dias;
            Console.Write("Ingresa el número de día para mostrar las ventas: ");
            if (int.TryParse(Console.ReadLine(), out dias))
            {
                detalleVentaDtos = utilitiesRepository.consultaDetalleDeVentas(dias);
                utilitiesRepository.MostrarFacturas(detalleVentaDtos).Write();
            }
            break;
        case "2":
            utilitiesRepository.MostarTotalVentas(detalleVentaDtos).Write();
            break;
        case "3":
            utilitiesRepository.MostarProductoMayorVenta(detalleVentaDtos).Write();
            break;
        case "4":
            utilitiesRepository.MostarLocalMayorVenta(detalleVentaDtos).Write();
            break;
        case "5":
            utilitiesRepository.MostrarMarcaMayorMargen(detalleVentaDtos).Write();
            break;
        case "6":
            utilitiesRepository.MostarLocalProductoMasVendido(detalleVentaDtos).Write();
            break;
        case "7":
            Console.WriteLine("Saliendo del programa...");
            return;
        default:
            Console.WriteLine("Opción inválida. Inténtalo nuevamente.");
            break;
    }

    Console.WriteLine();
}
