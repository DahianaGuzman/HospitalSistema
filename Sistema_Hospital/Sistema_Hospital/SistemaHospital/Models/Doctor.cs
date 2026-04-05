using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaHospital.Models
{   // ──────────────────────────────────────────────
    //  Doctor
    // ──────────────────────────────────────────────
    public class Doctor
    {
        public int Identificacion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public int Telefono { get; set; }
        public string Especialidad { get; set; } = string.Empty;
        public int LicenciaMedica { get; set; }

        // Constructor vacío requerido por CsvHelper
        public Doctor() { }

        public Doctor(int identificacion, string nombre, int edad, int telefono, string especialidad, int licenciaMedica)
        {
            Identificacion = identificacion;
            Nombre = nombre;
            Edad = edad;
            Telefono = telefono;
            Especialidad = especialidad;
            LicenciaMedica = licenciaMedica;
        }

        public override string ToString() => $"Dr. {Nombre} — {Especialidad} (Lic: {LicenciaMedica})";
    }
}