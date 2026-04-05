using System.Collections.Generic;
using System.Linq;
using SistemaHospital.Interfaces;
using SistemaHospital.Models;

namespace SistemaHospital.Repositories
{
    public class HospitalizacionRepository : CsvRepository<Hospitalizacion>, IAsignacionHabitacion
    {
        private const int TotalHabitaciones = 10;

        // ← constructor aquí, primero
        public HospitalizacionRepository() : base("hospitalizaciones.csv") { }

        public int ObtenerHabitacionDisponible()
        {
            var ocupadas = CargarTodos()
                .Where(h => !h.FechaAlta.HasValue)
                .Select(h => h.NumeroHabitacion)
                .ToHashSet();

            for (int i = 1; i <= TotalHabitaciones; i++)
                if (!ocupadas.Contains(i)) return i;

            return -1;
        }

        public bool HabitacionDisponible(int numeroHabitacion)
        {
            return !CargarTodos()
                .Where(h => !h.FechaAlta.HasValue)
                .Any(h => h.NumeroHabitacion == numeroHabitacion);
        }
    }
}
