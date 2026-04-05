using System;
using System.Collections.Generic;
using System.Text;
using SistemaHospital.Models;
using SistemaHospital.Repositories;


namespace SistemaHospital.Models
{
    // ──────────────────────────────────────────────
    //  Paciente — lleva su HistorialMedico consigo
    // ──────────────────────────────────────────────
    public class Paciente
    {
    public int Identificacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public int Telefono { get; set; }
    public DateTime FechaIngreso { get; set; }

    // El historial vive DENTRO del paciente, no en una lista aparte

    public HistorialMedico Historial { get; set; } = new();

    // Constructor vacío requerido por CsvHelper
        // Constructor vacío requerido por CsvHelper
        public Paciente() { }

        public Paciente(int identificacion, string nombre, int edad, int telefono, DateTime fechaIngreso)
        {
            Identificacion = identificacion;
            Nombre = nombre;
            Edad = edad;
            Telefono = telefono;
            FechaIngreso = fechaIngreso;
            Historial = new HistorialMedico(); // inicializa el historial vacío
        }

        public override string ToString() => $"{Nombre} (ID: {Identificacion})"; // Para mostrar en listados
    }
}
