using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConsoleDefontana
{
    public class Program
    {
        static void Main(string[] args)
        {
            string _connectionString = "Server = lab-defontana.caporvnn6sbh.us-east-1.rds.amazonaws.com; Database = Prueba; User Id = ReadOnly; Password = d*3PSf2MmRX9vJtA5sgwSphCVQ26*T53uU; TrustServerCertificate = True;";
            // Configurar el contexto de la base de datos
            var options = new DbContextOptionsBuilder<ConfigDbContext>()
                .UseSqlServer(_connectionString)
                .Options;
            var dbContext = new ConfigDbContext(options);
        }
    }
}
