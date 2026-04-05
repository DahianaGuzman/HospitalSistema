using System;
using System.Collections.Generic;
using SistemaHospital;
using SistemaHospital.Models;
using SistemaHospital.Repositories;

namespace AppConsole
{
    internal static class UICita
    {
        private static readonly CitaRepository _repo = new();

        // ── CRUD ────────────────────────────────────────────────────────────

        private static void Crear()
        {
            // 1. Elegir paciente (fuente única: PacienteConHistorialRepository via UIPaciente)
            Console.WriteLine("Seleccione el paciente para la cita:");
            var paciente = UIPaciente.Seleccionar();
            if (paciente == null) { Console.WriteLine("Operación cancelada."); return; }

            // 2. Elegir doctor
            Console.WriteLine("Seleccione el doctor para la cita:");
            var doctor = UIDoctor.Seleccionar();
            if (doctor == null) { Console.WriteLine("Operación cancelada."); return; }

            // 3. Fecha
            Console.Write("Fecha y hora (yyyy-MM-dd HH:mm): ");
            var texto = Console.ReadLine();
            var fecha = DateTime.TryParse(texto, out var f) ? f : DateTime.Now;

            // 4. Motivo
            Console.Write("Motivo de la cita: ");
            var motivo = Console.ReadLine() ?? "";

            // 5. Guardar — solo guardamos IDs, no objetos completos
            var citas = _repo.CargarTodos();
            int nuevoId = citas.Count > 0 ? citas[^1].Id + 1 : 1;
            var nuevaCita = new Cita(nuevoId, fecha, paciente.Identificacion, doctor.Identificacion, motivo);
            citas.Add(nuevaCita);
            _repo.GuardarTodos(citas);
            Console.WriteLine($"Cita #{nuevoId} registrada: {paciente.Nombre} con {doctor.Nombre} el {fecha:g}.");
        }

        private static void Listar()
        {
            var citas = _repo.CargarTodos();
            if (citas.Count == 0) { Console.WriteLine("No hay citas registradas."); return; }

            Console.WriteLine("\n--- Citas ---");
            for (int i = 0; i < citas.Count; i++)
            {
                var c = citas[i];
                Console.WriteLine($"[{i}] ID:{c.Id} | {c.FechaCita:g} | Paciente ID:{c.IdPaciente} | Doctor ID:{c.IdDoctor} | Motivo: {c.Motivo}");
            }
        }

        private static void Actualizar()
        {
            var citas = _repo.CargarTodos();
            Listar();
            if (citas.Count == 0) return;

            Console.Write("Índice a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= citas.Count)
            { Console.WriteLine("Índice inválido."); return; }

            var c = citas[idx];
            Console.WriteLine("Deje vacío para mantener el valor actual.");

            Console.Write($"Nueva fecha ({c.FechaCita:g}): ");
            var texto = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(texto) && DateTime.TryParse(texto, out var f)) c.FechaCita = f;

            Console.Write($"Nuevo motivo ({c.Motivo}): ");
            var motivo = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(motivo)) c.Motivo = motivo;

            _repo.GuardarTodos(citas);
            Console.WriteLine("Cita actualizada.");
        }

        private static void Eliminar()
        {
            var citas = _repo.CargarTodos();
            Listar();
            if (citas.Count == 0) return;

            Console.Write("Índice a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= citas.Count)
            { Console.WriteLine("Índice inválido."); return; }

            citas.RemoveAt(idx);
            _repo.GuardarTodos(citas);
            Console.WriteLine("Cita eliminada.");
        }

        // ── Menú ─────────────────────────────────────────────────────────────

        public static void Gestionar()
        {
            const string menu = """

                --- Gestión de Citas ---
                1. Crear cita
                2. Listar citas
                3. Actualizar cita
                4. Eliminar cita
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
                    case "3": Actualizar(); break;
                    case "4": Eliminar(); break;
                    case "5": return;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            } while (true);
        }
    }
}