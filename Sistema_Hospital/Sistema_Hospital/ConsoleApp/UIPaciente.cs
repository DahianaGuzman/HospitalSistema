using System;
using System.Collections.Generic;
using SistemaHospital;
using SistemaHospital.Models;
using SistemaHospital.Repositories;

namespace AppConsole
{
    internal static class UIPaciente
    {
        // UNA SOLA fuente de verdad: el repositorio
        // No hay lista flotante de pacientes en esta clase. Cada vez que se necesita, se carga desde el repositorio.
        private static readonly PacienteHistorialRepository _repo = new();

        // ── CRUD ────────────────────────────────────────────────────────────

        private static void Crear()
        {
            var pacientes = _repo.CargarTodos();
            var p = LeerDesdConsola();
            pacientes.Add(p);
            _repo.GuardarTodos(pacientes);
            Console.WriteLine($"Paciente '{p.Nombre}' creado correctamente.");
        }

        private static void Listar()
        {
            var pacientes = _repo.CargarTodos();
            if (pacientes.Count == 0) { Console.WriteLine("No hay pacientes registrados."); return; }

            Console.WriteLine("\n--- Pacientes ---");
            for (int i = 0; i < pacientes.Count; i++)
            {
                var p = pacientes[i];
                Console.WriteLine($"[{i}] {p.Nombre} — ID: {p.Identificacion} | Edad: {p.Edad} | Tel: {p.Telefono} | Ingreso: {p.FechaIngreso:d}");
            }
        }

        private static void Actualizar()
        {
            var pacientes = _repo.CargarTodos();
            Listar();
            if (pacientes.Count == 0) return;

            Console.Write("Índice a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= pacientes.Count)
            { Console.WriteLine("Índice inválido."); return; }

            var p = pacientes[idx];
            Console.WriteLine("Deje vacío para mantener el valor actual.");

            // Id no se cambia, fecha de ingreso tampoco es relevante actualizar
            Console.Write($"Nombre ({p.Nombre}): ");
            var nombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nombre)) p.Nombre = nombre;

            Console.Write($"Edad ({p.Edad}): ");
            if (int.TryParse(Console.ReadLine(), out var edad)) p.Edad = edad;

            Console.Write($"Teléfono ({p.Telefono}): ");
            if (int.TryParse(Console.ReadLine(), out var tel)) p.Telefono = tel;

            _repo.GuardarTodos(pacientes);
            Console.WriteLine("Paciente actualizado.");
        }

        private static void Eliminar()
        {
            var pacientes = _repo.CargarTodos();
            Listar();
            if (pacientes.Count == 0) return;

            Console.Write("Índice a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= pacientes.Count)
            { Console.WriteLine("Índice inválido."); return; }

            var nombre = pacientes[idx].Nombre;
            pacientes.RemoveAt(idx);
            _repo.GuardarTodos(pacientes);
            Console.WriteLine($"Paciente '{nombre}' eliminado.");
        }

        // ── Historial del paciente ───────────────────────────────────────────

        private static void GestionarHistorial()
        {
            var pacientes = _repo.CargarTodos();
            Listar();
            if (pacientes.Count == 0) return;

            Console.Write("Índice del paciente: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= pacientes.Count)
            { Console.WriteLine("Índice inválido."); return; }

            var p = pacientes[idx];
            UIHistorial.Gestionar(p.Historial);   // delega a UIHistorial
            _repo.GuardarTodos(pacientes);         // guarda el paciente con historial actualizado
        }

        // ── Métodos de apoyo para otras UIs ─────────────────────────────────

        /// Permite elegir un paciente existente o crear uno nuevo. Usado por UICita y UIHospitalizacion
        public static Paciente? Seleccionar() // Devuelve null si no hay pacientes y el usuario no quiere crear uno nuevo
        {
            var pacientes = _repo.CargarTodos();

            if (pacientes.Count > 0)
            {
                Console.WriteLine("\nPacientes disponibles:");
                for (int i = 0; i < pacientes.Count; i++)
                    Console.WriteLine($"[{i}] {pacientes[i]}");

                Console.Write("Índice ('n' = crear nuevo): ");
                var sel = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(sel) && sel.ToLower() != "n"
                    && int.TryParse(sel, out var idx) && idx >= 0 && idx < pacientes.Count)
                    return pacientes[idx];
            }

            // Crear nuevo
            var nuevo = LeerDesdConsola();
            pacientes.Add(nuevo);
            _repo.GuardarTodos(pacientes);
            return nuevo;
        }

        /// Lee los datos de un paciente desde la consola y devuelve el objeto.
        public static Paciente LeerDesdConsola()
        {
            Console.Write("Identificación (número): ");
            int id = int.TryParse(Console.ReadLine(), out var tmpId) ? tmpId : 0;

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("Edad: ");
            int edad = int.TryParse(Console.ReadLine(), out var tmpE) ? tmpE : 0;

            Console.Write("Teléfono: ");
            int tel = int.TryParse(Console.ReadLine(), out var tmpT) ? tmpT : 0;

            Console.Write("Fecha de ingreso (yyyy-MM-dd) [Enter = hoy]: ");
            var fi = Console.ReadLine();                                // Si el usuario deja vacío o ingresa un formato inválido, se asigna la fecha actual
            var fecha = DateTime.TryParse(fi, out var f) ? f : DateTime.Now;// Permite flexibilidad al usuario y garantiza que siempre haya una fecha válida.

            return new Paciente(id, nombre, edad, tel, fecha);
        }

        // ── Menú ─────────────────────────────────────────────────────────────

        public static void Gestionar()
        {
            const string menu = """

                --- Gestión de Pacientes ---
                1. Crear paciente
                2. Listar pacientes
                3. Actualizar paciente
                4. Eliminar paciente
                5. Gestionar historial médico de un paciente
                6. Volver
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
                    case "5": GestionarHistorial(); break;
                    case "6": return;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }while (true);
        }
    }
}