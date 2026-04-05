using System;
using System.Collections.Generic;
using SistemaHospital.Repositories;
using System.Text;

namespace SistemaHospital.Models
{
    public class Persona
    {
        // atributos de la clase persona
        public string Nombre { get; set; }
        public int Identificacion { get; set; }
        public int Edad { get; set; }
        public int Telefono { get; set; }

        // constructor de la clase persona
        public Persona(
            string nombre,
            int identificacion,
            int edad,
            int telefono)
        {
            Nombre = nombre;
            Identificacion = identificacion;
            Edad = edad;
            Telefono = telefono;
        }
    }
}