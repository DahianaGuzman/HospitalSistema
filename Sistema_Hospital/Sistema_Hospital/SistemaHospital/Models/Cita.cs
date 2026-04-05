using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaHospital.Models
{
    // ──────────────────────────────────────────────
    //  Cita — referencia IDs, no objetos completos
    //  (los objetos completos se cargan desde sus repos)
    // ──────────────────────────────────────────────
    public class Cita
    {
        public int Id { get; set; }
        public DateTime FechaCita { get; set; }
        public int IdPaciente { get; set; }   // FK → Paciente.Identificacion
        public int IdDoctor { get; set; }   // FK → Doctor.Identificacion
        public string Motivo { get; set; } = string.Empty;

        // Constructor vacío requerido por CsvHelper
        public Cita() { }

        public Cita(int id, DateTime fechaCita, int idPaciente, int idDoctor, string motivo = "")
        {
            Id = id;
            FechaCita = fechaCita;
            IdPaciente = idPaciente;
            IdDoctor = idDoctor;
            Motivo = motivo;
        }
    }
}