using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using System;


using SistemaHospital.Models;

namespace SistemaHospital.Repositories
{
    /// <summary>
    /// El historial no puede guardarse directo en CSV porque tiene List&lt;string&gt; internas.
    /// Usamos un DTO (PacienteDto) que serializa todo a campos planos, y lo convertimos
    /// a Paciente al cargar. Así el CSV sigue siendo simple y legible.
    /// </summary>
    public class PacienteHistorialRepository
    {
        private const string FilePath = "pacientes.csv";

        // DTO plano que CsvHelper puede leer/escribir sin problema
        private class PacienteDto
        {
            public int Identificacion { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public int Edad { get; set; }
            public int Telefono { get; set; }
            public string FechaIngreso { get; set; } = string.Empty;
            // Listas guardadas como texto separado por '|'
            public string Diagnosticos { get; set; } = string.Empty;
            public string Tratamientos { get; set; } = string.Empty;
            public string Observaciones { get; set; } = string.Empty;
        }

        public List<Paciente> CargarTodos()
        {
            if (!File.Exists(FilePath))
                //return new List<Paciente>();
                return [];

            using var reader = new StreamReader(FilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<PacienteDto>().Select(dto => new Paciente(
                dto.Identificacion,
                dto.Nombre,
                dto.Edad,
                dto.Telefono,
                System.DateTime.TryParse(dto.FechaIngreso, out var fi) ? fi : System.DateTime.Now
            )
            {
                Historial = new HistorialMedico(  //VERIFICAR QUE EL HISTORIAL SE CARGA CORRECTAMENTE DESDE EL DTO
                     SplitLista(dto.Diagnosticos),
                     SplitLista(dto.Tratamientos),
                     SplitLista(dto.Observaciones)
                 )
                //YA QUE EL CONSTRUCTOR DE PACIENTE INICIALIZA UN HISTORIAL VACÍO
            }).ToList();

           
        }

        public void GuardarTodos(List<Paciente> pacientes)
        {
            var dtos = pacientes.Select(p => new PacienteDto
            {
                Identificacion = p.Identificacion,
                Nombre = p.Nombre,
                Edad = p.Edad,
                Telefono = p.Telefono,
                FechaIngreso = p.FechaIngreso.ToString("o"),
                Diagnosticos = string.Join("|", p.Historial.Diagnosticos),
                Tratamientos = string.Join("|", p.Historial.Tratamientos),
                Observaciones = string.Join("|", p.Historial.Observaciones)
            }).ToList();

            using var writer = new StreamWriter(FilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(dtos);
        }

        private static List<string> SplitLista(string valor) =>
            string.IsNullOrWhiteSpace(valor)
                ? []
                : [.. valor.Split('|').Where(s => !string.IsNullOrEmpty(s))];

    }
}