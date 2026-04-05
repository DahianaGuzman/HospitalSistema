using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace SistemaHospital.Repositories
{
    
    /// Repositorio genérico CSV. Herédalo para no repetir CargarTodos/GuardarTodos en cada entidad.
    public abstract class CsvRepository<T>
    {
        protected readonly string FilePath; // Ruta del archivo CSV específico para esta entidad

        protected CsvRepository(string filePath)
        {
            FilePath = filePath;
        }

        /// Carga todos los registros del CSV. Devuelve lista vacía si el archivo no existe.
        public List<T> CargarTodos()
        {
            if (!File.Exists(FilePath))
                return new List<T>();

            using var reader = new StreamReader(FilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);// CsvHelper lanzará excepción si el formato es incorrecto, lo dejamos así para detectar errores temprano.
            return new List<T>(csv.GetRecords<T>()); // Convertimos a List<T> para facilitar su uso
        }

        /// Sobreescribe el CSV completo con la lista proporcionada.
        public void GuardarTodos(List<T> registros)
        {
            using var writer = new StreamWriter(FilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(registros);
        }
    }
}