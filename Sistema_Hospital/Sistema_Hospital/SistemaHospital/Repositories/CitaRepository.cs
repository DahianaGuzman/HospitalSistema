using System;
using System.Collections.Generic;
using System.Text;
using SistemaHospital.Models;

namespace SistemaHospital.Repositories
{
    public class CitaRepository : CsvRepository<Cita>
    {
        public CitaRepository() : base("citas.csv") { }
    }
}