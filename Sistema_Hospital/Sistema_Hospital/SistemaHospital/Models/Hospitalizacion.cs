using System;

namespace SistemaHospital.Models
{
    // ──────────────────────────────────────────────
    //  Hospitalizacion — referencia IDs también
    // ──────────────────────────────────────────────
    public class Hospitalizacion
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }   // FK → Paciente.Identificacion
        public int IdDoctor { get; set; }   // FK → Doctor.Identificacion
        public int NumeroHabitacion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaAlta { get; set; }

        // Constructor vacío requerido por CsvHelper
        public Hospitalizacion() { }

        public Hospitalizacion(int id, int idPaciente, int idDoctor, int numeroHabitacion, DateTime fechaIngreso)
        {
            Id = id;
            IdPaciente = idPaciente;
            IdDoctor = idDoctor;
            NumeroHabitacion = numeroHabitacion;
            FechaIngreso = fechaIngreso;
        }

        public Hospitalizacion(Paciente paciente, Habitacion habitacion, DateTime now)
        {
        }

        public bool EstaActiva => !FechaAlta.HasValue;

        public void DarDeAlta(DateTime fecha) => FechaAlta = fecha;
    }
}