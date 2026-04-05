using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SistemaHospital.Models
{
    public class Hospital 
    {
        public List<Doctor> Doctores { get; set; }
        public List<Habitacion> Habitaciones { get; set; }
        public List<Paciente> Pacientes { get; set; } // Composición
        public List<Hospitalizacion> Hospitalizaciones { get; set; }

        // Composición con Habitacion (el hospital las crea al inicializar)
        public Hospital(int cantidadHabitaciones)
        {
            Doctores = new List<Doctor>();
            Pacientes = new List<Paciente>();
            Habitaciones = new List<Habitacion>();
            Hospitalizaciones = new List<Hospitalizacion>();


        // cambio por verificar
            for (int i = 1; i <= cantidadHabitaciones; i++)
            {
                Habitaciones.Add(new Habitacion(i));
            }
        }

        // Agregación con Doctor (se agregan doctores ya existentes)
        public void AgregarDoctor(Doctor doctor)
        {
            Doctores.Add(doctor);
        }

        // Composición con Paciente y gestión de hospitalizaciones
        public void AgregarPaciente(Paciente paciente)
        {
            Pacientes.Add(paciente);
        }
        //public Hospitalizacion RegistrarHospitalizacion(int id, int idPaciente, int idDoctor, int numeroHabitacion, DateTime fechaIngreso)
        public Hospitalizacion RegistrarHospitalizacion(Paciente paciente, Habitacion habitacion, Doctor doctorSupervisor)//null
        {
            if (habitacion.Ocupada)
                throw new Exception("Habitación ya ocupada");


            var hospitalizacion = new Hospitalizacion(paciente, habitacion, DateTime.Now);
            hospitalizacion.IdDoctor = doctorSupervisor?.Identificacion ?? 0;
            Hospitalizaciones.Add(hospitalizacion);
            return hospitalizacion;
        }
    }
   }
