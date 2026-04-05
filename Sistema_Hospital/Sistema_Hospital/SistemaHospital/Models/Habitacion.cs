using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaHospital.Models
{
    public class Habitacion
    {
        public int NumeroHabitacion { get; set; }
        public bool Ocupada { get; set; }
        public Habitacion(int numeroHabitacion)
        {
            NumeroHabitacion = numeroHabitacion;
            Ocupada = false;
        }

        //public Habitacion(int numeroHabitacion, bool ocupada)
        //{
        //    NumeroHabitacion = numeroHabitacion;
        //    Ocupada = ocupada;
        //}
    }
}
