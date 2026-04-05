using System;
using System.Collections.Generic;
using System.Text;
using SistemaHospital.Models;

namespace SistemaHospital.Repositories
{
    public class DoctorRepository : CsvRepository<Doctor>
    {
        public DoctorRepository() : base("doctores.csv") { }
    }
}