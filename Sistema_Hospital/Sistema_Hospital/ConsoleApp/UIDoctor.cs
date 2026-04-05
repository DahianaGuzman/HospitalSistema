using System;
using SistemaHospital.Models;
using SistemaHospital.Repositories;

namespace AppConsole
{
    internal static class UIDoctor
    {
        private static readonly DoctorRepository _repo = new();

        // ── CRUD ────────────────────────────────────────────────────────────

        private static void Crear()
        {
            var doctores = _repo.CargarTodos();
            var d = LeerDesdeConsola();
            doctores.Add(d);
            _repo.GuardarTodos(doctores);
            Console.WriteLine($"Doctor '{d.Nombre}' creado correctamente.");
        }

        private static void Listar()
        {
            var doctores = _repo.CargarTodos();
            if (doctores.Count == 0) { Console.WriteLine("No hay doctores registrados."); return; }

            Console.WriteLine("\n--- Doctores ---");
            for (int i = 0; i < doctores.Count; i++)
            {
                var d = doctores[i];
                Console.WriteLine($"[{i}] {d.Nombre} — ID: {d.Identificacion} | Esp: {d.Especialidad} | Lic: {d.LicenciaMedica}");
            }
        }

        private static void Actualizar()
        {
            var doctores = _repo.CargarTodos();
            Listar();
            if (doctores.Count == 0) return;

            Console.Write("Índice a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= doctores.Count)
            { Console.WriteLine("Índice inválido."); return; }

            var d = doctores[idx];
            Console.WriteLine("Deje vacío para mantener el valor actual.");

            // Id y licencia medica no se cambian, edad tampoco es muy relevante en un doctor 

            Console.Write($"Nombre ({d.Nombre}): ");
            var nombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nombre)) d.Nombre = nombre;

            Console.Write($"Especialidad ({d.Especialidad}): ");
            var esp = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(esp)) d.Especialidad = esp;

            Console.Write($"Teléfono ({d.Telefono}): ");
            if (int.TryParse(Console.ReadLine(), out var tel)) d.Telefono = tel;

            _repo.GuardarTodos(doctores);
            Console.WriteLine("Doctor actualizado.");
        }

        private static void Eliminar()
        {
            var doctores = _repo.CargarTodos();
            Listar();
            if (doctores.Count == 0) return;

            Console.Write("Índice a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 0 || idx >= doctores.Count)
            { Console.WriteLine("Índice inválido."); return; }

            var nombre = doctores[idx].Nombre;
            doctores.RemoveAt(idx);
            _repo.GuardarTodos(doctores);
            Console.WriteLine($"Doctor '{nombre}' eliminado.");
        }

        // ── Apoyo para otras UIs ─────────────────────────────────────────────

        /// <summary>Permite elegir un doctor existente o crear uno nuevo. Usado por UICita y UIHospitalizacion.</summary>
        public static Doctor? Seleccionar()
        {
            var doctores = _repo.CargarTodos();

            if (doctores.Count > 0)
            {
                Console.WriteLine("\nDoctores disponibles:");
                for (int i = 0; i < doctores.Count; i++)
                    Console.WriteLine($"[{i}] {doctores[i]}");

                Console.Write("Índice ('n' = crear nuevo, 'c' = cancelar): ");
                var sel = Console.ReadLine();
                if (sel?.ToLower() == "c") return null;
                if (!string.IsNullOrWhiteSpace(sel) && sel.ToLower() != "n"
                    && int.TryParse(sel, out var idx) && idx >= 0 && idx < doctores.Count)
                    return doctores[idx];
            }

            // Crear nuevo
            var nuevo = LeerDesdeConsola();
            doctores.Add(nuevo);
            _repo.GuardarTodos(doctores);
            return nuevo;
        }

        public static Doctor LeerDesdeConsola()
        {
            Console.Write("Identificación (número): ");
            int id = int.TryParse(Console.ReadLine(), out var tmpId) ? tmpId : 0;

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("Edad: ");
            int edad = int.TryParse(Console.ReadLine(), out var tmpE) ? tmpE : 0;

            Console.Write("Teléfono: ");
            int tel = int.TryParse(Console.ReadLine(), out var tmpT) ? tmpT : 0;

            Console.Write("Especialidad: ");
            string esp = Console.ReadLine() ?? "";

            Console.Write("Licencia médica (número): ");
            int lic = int.TryParse(Console.ReadLine(), out var tmpL) ? tmpL : 0;

            return new Doctor(id, nombre, edad, tel, esp, lic);
        }

        // ── Menú ─────────────────────────────────────────────────────────────

        public static void Gestionar()
        {
            const string menu = """

                --- Gestión de Doctores ---
                1. Crear doctor
                2. Listar doctores
                3. Actualizar doctor
                4. Eliminar doctor
                5. Volver
                Opción: 
                """;

            while (true)
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
            }
        }
    }
}