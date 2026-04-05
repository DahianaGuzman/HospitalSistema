using System;
using System.Collections.Generic;
using System.Linq;
using SistemaHospital;
using SistemaHospital.Models;
using SistemaHospital.Repositories;

namespace AppConsole
{
    internal static class UIHospitalizacion
    {
        private static readonly HospitalizacionRepository _repo = new();

        // Habitaciones disponibles del hospital (puedes ajustar la cantidad)
       // private const int TotalHabitaciones = 10; (se quito al implementar la interfaz)

        // ── CRUD ────────────────────────────────────────────────────────────

        private static void Crear()
        {
            var hospitalizaciones = _repo.CargarTodos();

            // 1. Elegir paciente
            Console.WriteLine("Seleccione el paciente a hospitalizar:");
            var paciente = UIPaciente.Seleccionar();
            if (paciente == null) { Console.WriteLine("Operación cancelada."); return; }

            // 2. Elegir doctor supervisor
            Console.WriteLine("Seleccione el doctor supervisor:");
            var doctor = UIDoctor.Seleccionar();
            if (doctor == null) { Console.WriteLine("Operación cancelada."); return; }

            // 3. Buscar habitación libre usando la interfaz IAsignacionHabitacion
            int habitacionLibre = _repo.ObtenerHabitacionDisponible(); 
            if (habitacionLibre == -1)
            {
                Console.WriteLine("No hay habitaciones disponibles.");
                return;
            }

            // 4. Registrar — UNA sola vez, con todos los datos
            int nuevoId = hospitalizaciones.Count > 0 ? hospitalizaciones[^1].Id + 1 : 1; // Genera un nuevo ID secuencial
            var hosp = new Hospitalizacion(nuevoId, paciente.Identificacion, doctor.Identificacion, habitacionLibre, DateTime.Now); 
            hospitalizaciones.Add(hosp);
            _repo.GuardarTodos(hospitalizaciones);

            Console.WriteLine($"Paciente '{paciente.Nombre}' hospitalizado en habitación {habitacionLibre} bajo supervisión de {doctor.Nombre}.");
        }

        private static void Listar()
        {
            var hs = _repo.CargarTodos(); // Carga todas las hospitalizaciones desde el repositorio
            if (hs.Count == 0) { Console.WriteLine("No hay hospitalizaciones registradas."); return; }

            Console.WriteLine("\n--- Hospitalizaciones ---");
            for (int i = 0; i < hs.Count; i++)
            {
                var h = hs[i];
                var estado = h.EstaActiva ? "Activa" : $"Alta: {h.FechaAlta:d}";
                Console.WriteLine($"[{i}] ID:{h.Id} | Pac ID:{h.IdPaciente} | Dr ID:{h.IdDoctor} | Hab:{h.NumeroHabitacion} | Ingreso:{h.FechaIngreso:d} | {estado}");
            }
        }

        private static void DarDeAlta()
        {
            var hs = _repo.CargarTodos();
            Listar();
            if (hs.Count == 0) return;

            Console.Write("Índice de la hospitalización a dar de alta: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= hs.Count)
            { Console.WriteLine("Índice inválido."); return; }

            if (!hs[idx].EstaActiva)
            { Console.WriteLine("Esa hospitalización ya tiene alta registrada."); return; }

            hs[idx].DarDeAlta(DateTime.Now);
            _repo.GuardarTodos(hs);
            Console.WriteLine("Alta registrada correctamente.");
        }

        private static void Eliminar()
        {
            var hs = _repo.CargarTodos();
            Listar();
            if (hs.Count == 0) return;

            Console.Write("Índice a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= hs.Count)
            { Console.WriteLine("Índice inválido."); return; }

            hs.RemoveAt(idx);
            _repo.GuardarTodos(hs);
            Console.WriteLine("Hospitalización eliminada.");
        }

        // ── Menú ─────────────────────────────────────────────────────────────

        public static void Gestionar()
        {
            const string menu = """

                --- Gestión de Hospitalizaciones ---
                1. Hospitalizar paciente
                2. Listar hospitalizaciones
                3. Dar de alta
                4. Eliminar hospitalización
                5. Volver
                Opción: 
                """;

            do
            {
                Console.Write(menu);
                switch (Console.ReadLine())
                {
                    case "1": Crear(); break;
                    case "2": Listar(); break;
                    case "3": DarDeAlta(); break;
                    case "4": Eliminar(); break;
                    case "5": return;
                    default: Console.WriteLine("Opción inválida."); break;
                }

            }while (true);
        }
    }
}
