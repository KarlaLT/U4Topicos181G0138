using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using SQLite;

namespace ProyectoU4.Model
{
    public class CatalogoCitas
    {
        public SQLiteConnection conexion { get; set; }
        string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/BdAgendaDentista.db";
        public IEnumerable<Citas> GetCitas()
        {
            return conexion.Table<Citas>().Where(x => x.Fecha > DateTime.Now.Date);
        }
        public IEnumerable<Citas> GetCitasHoy()
        {
            return conexion.Table<Citas>().Where(x => x.Fecha == DateTime.Now.Date);
        }
        public IEnumerable<Citas> GetOldCitas()
        {
            return conexion.Table<Citas>().Where(x => x.Fecha < (DateTime.Now.Date));
        }
        public Citas GetCitaById(Citas c)
        {
            return conexion.Table<Citas>().FirstOrDefault(x => x.Id == c.Id);
        }

        public void Insert(Citas c)
        {
            conexion.Insert(c);
        }
        public void Update(Citas c)
        {
            var cita = GetCitaById(c);
            cita.Cliente = c.Cliente;
            cita.Fecha = c.Fecha;
            cita.Hora = c.Hora;
            cita.Doctor = c.Doctor;
            cita.Lugar = c.Lugar;
            cita.Servicio = c.Servicio;

            conexion.Update(cita);
        }
        public void Delete(Citas c)
        {
            conexion.Delete(c);
        }
        public bool Validate(Citas c)
        {
            if (string.IsNullOrWhiteSpace(c.Cliente))
                throw new ArgumentException("Ingrese su nombre.");
            if (string.IsNullOrWhiteSpace(c.Servicio))
                throw new ArgumentException("Ingrese el servicio que requiere.");
            if (string.IsNullOrWhiteSpace(c.Lugar))
                throw new ArgumentException("Ingrese el lugar donde requiere su cita.");
            if (string.IsNullOrWhiteSpace(c.Doctor))
                throw new ArgumentException("Ingrese el doctor con quien tendrá consulta.");
            if (c.Fecha < DateTime.Now.Date)
                throw new ArgumentException("La fecha no puede ser menor a la actual.");


            return true;
        }

        private void VerificarBD()
        {
            if (!File.Exists(ruta))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream("ProyectoCitasDentista.Data.BdAgendaDentista.db");
                var archivo = File.Create(ruta);
                stream.CopyTo(archivo);
                stream.Close();
                archivo.Close();
            }
        }
        public CatalogoCitas()
        {
            VerificarBD();
            conexion = new SQLiteConnection(ruta);
        }
    }
}
