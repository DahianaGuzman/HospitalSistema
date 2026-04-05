using System;
using System.Collections.Generic;

namespace SistemaHospital.Models
{
    // ──────────────────────────────────────────────
    //  HistorialMedico — pertenece a un Paciente
    // ──────────────────────────────────────────────
    public class HistorialMedico
    {
        public List<string> Diagnosticos { get; set; } = new();
        public List<string> Tratamientos { get; set; } = new();
        public List<string> Observaciones { get; set; } = new();

        // Constructor vacío requerido por CsvHelper
        public HistorialMedico() { }

        public HistorialMedico(List<string> diagnosticos, List<string> tratamientos, List<string> observaciones)
        {
            Diagnosticos = diagnosticos;
            Tratamientos = tratamientos;
            Observaciones = observaciones;
        }
    }
}