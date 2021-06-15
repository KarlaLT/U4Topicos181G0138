using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ProyectoU4.Model
{
    public class Citas
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Cliente { get; set; }
        public string Servicio { get; set; }
        public string Lugar { get; set; }
        public string Doctor { get; set; }
    }
}
