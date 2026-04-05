using System;
using System.Collections.Generic;
using SistemaHospital;
using SistemaHospital.Models;
using SistemaHospital.Repositories;

namespace AppConsole
{
  
    /// HISTORIAL NO REQUIERE menú propio. UIPaciente la llama pasándole el historial del paciente seleccionado.
    /// El historial ya NO existe como lista flotante — vive dentro del Paciente.
    internal static class UIHistorial
    {
        //public static void Gestionar(PacienteHistorialRepository Historial)
        public static void Gestionar(HistorialMedico historial)

        {
            const string menu = """

                --- Historial Médico ---
                1. Ver historial
                2. Agregar diagnóstico
                3. Agregar tratamiento
                4. Agregar observación
                5. Limpiar historial
                6. Volver
                Opción: 
                """;

            while (true)
            {
                Console.Write(menu);
                switch (Console.ReadLine())
                {
                    case "1": Ver(historial); break;
                    case "2": AgregarItem(historial.Diagnosticos, "diagnóstico"); break;
                    case "3": AgregarItem(historial.Tratamientos, "tratamiento"); break;
                    case "4": AgregarItem(historial.Observaciones, "observación"); break;
                    case "5": Limpiar(historial); break;
                    case "6": return;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }
        }

        //private static void Ver(PacienteHistorialRepository h)
        private static void Ver(HistorialMedico h)
        {
            Console.WriteLine("\n  Diagnósticos:");
            if (h.Diagnosticos.Count == 0) Console.WriteLine("    (ninguno)");
            else h.Diagnosticos.ForEach(d => Console.WriteLine($"    - {d}"));

            Console.WriteLine("  Tratamientos:");
            if (h.Tratamientos.Count == 0) Console.WriteLine("    (ninguno)");
            else h.Tratamientos.ForEach(t => Console.WriteLine($"    - {t}"));

            Console.WriteLine("  Observaciones:");
            if (h.Observaciones.Count == 0) Console.WriteLine("    (ninguna)");
            else h.Observaciones.ForEach(o => Console.WriteLine($"    - {o}"));
        }

        // función genérica para agregar a cualquiera de las tres listas del historial
        private static void AgregarItem(List<string> lista, string tipo) // tipo es "diagnóstico", "tratamiento" u "observación"
        
        {                                                                
            Console.WriteLine($"Ingrese {tipo}s (línea vacía para terminar):"); 
            while (true)
            {
                Console.Write("  > ");
                var linea = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(linea)) break;
                lista.Add(linea);
            }
            Console.WriteLine($"{tipo.Substring(0, 1).ToUpper()}{tipo.Substring(1)} agregado(s).");// Ej: "Diagnóstico agregado(s)."
            // Nota: no guardamos en CSV aquí, el historial es parte del paciente y se guarda cuando se guarda el paciente completo.
        }

        //private static void Limpiar(PacienteHistorialRepository h)
        private static void Limpiar(HistorialMedico h)
        {
            Console.Write("¿Limpiar todo el historial? (s/n): ");
            if (Console.ReadLine()?.ToLower() == "s")
            {
                h.Diagnosticos.Clear();
                h.Tratamientos.Clear();
                h.Observaciones.Clear();
                Console.WriteLine("Historial limpiado.");
            }
        }
    }
}