using System;
using SistemaHospital;
using SistemaHospital.Models;
using SistemaHospital.Repositories;

namespace AppConsole
{
    public static class Program
    {

        static void Main(string[] args)
        {
            string menu =
"""
 
                ╔══════════════════════════════╗
                ║    Sistema Hospital          ║
                ╠══════════════════════════════╣
                ║  1. Pacientes                ║
                ║  2. Doctores                 ║
                ║  3. Citas                    ║
                ║  4. Hospitalizaciones        ║
                ║  5. Salir                    ║
                ╚══════════════════════════════╝
                Opción: 
                """;

            do
            {
                Console.Write(menu);
                var opt = Console.ReadLine();
                switch (opt)
                {
                    case "1":
                        UIPaciente.Gestionar();
                        break;
                    case "2":
                        UIDoctor.Gestionar();
                        break;
                    case "3":
                        UICita.Gestionar();
                        break;
                    case "4":
                        UIHospitalizacion.Gestionar();
                        break;
                    case "5":
                        Console.WriteLine("¡Hasta luego!");
                        return;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

            } while (true);
        }
    }
}
