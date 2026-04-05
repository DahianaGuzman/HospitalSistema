
// En SistemaHospital/Interfaces/IAsignacionHabitacion.cs
namespace SistemaHospital.Interfaces
{
    public interface IAsignacionHabitacion
    {
        int ObtenerHabitacionDisponible();
        bool HabitacionDisponible(int numeroHabitacion);
    }
}