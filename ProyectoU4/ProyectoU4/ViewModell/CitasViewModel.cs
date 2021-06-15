using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using ProyectoU4.Model;
using ProyectoU4.View;
using System.Collections.ObjectModel;
using Xamarin.Forms; 

namespace ProyectoU4.ViewModell
{
    public class CitasViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        AgregarView agregarView;
        EditarView editarView;
        CitasView citasView;
        VerCitaView citaView;

        CatalogoCitas catalogo = new CatalogoCitas();

        public ICommand VerAgregarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand VerCitasCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand CancelarEdElCommand { get; set; }
        public ICommand CitaCommand { get; set; }

        private string error;
        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error"));
            }
        }

        private Citas cita;
        public Citas Cita
        {
            get { return cita; }
            set
            {
                cita = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cita"));
            }
        }


        private ObservableCollection<Citas> citashoy;
        public ObservableCollection<Citas> CitasHoy
        {
            get { return citashoy; }
            set
            {
                citashoy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CitasHoy"));
            }
        }


        private ObservableCollection<Citas> citasREST;
        public ObservableCollection<Citas> CitasREST
        {
            get { return citasREST; }
            set
            {
                citasREST = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CitasREST"));
            }
        }
        private ObservableCollection<Citas> citasold;
        public ObservableCollection<Citas> CitasOld
        {
            get { return citasold; }
            set
            {
                citasold = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CitasOld"));
            }
        }
        private void VerAgregar()
        {
            if (agregarView == null)
                agregarView = new AgregarView() { BindingContext = this };

            Application.Current.MainPage.Navigation.PushModalAsync(agregarView);
            Cita = new Citas();
            CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
            CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
            Error = "";
        }
        public void Agregar()
        {
            try
            {
                if (catalogo.Validate(Cita))
                {
                    Error = "";
                    catalogo.Insert(Cita);
                    CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
                    CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
                  

                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        private void VerEditar(Citas c)
        {
            if (editarView == null)
                editarView = new EditarView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PopModalAsync();
            Application.Current.MainPage.Navigation.PushModalAsync(editarView);
            CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
            CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
            Error = "";
            if (c != null)
            {
                Cita = new Citas()
                {
                    Cliente = c.Cliente,
                    Fecha = c.Fecha,
                    Hora = c.Hora,
                    Id = c.Id,
                    Lugar = c.Lugar,
                    Servicio = c.Servicio,
                    Doctor = c.Doctor
                };
            }
        }
        public void Editar()
        {
            try
            {
                if (catalogo.Validate(Cita))
                {
                    Error = "";
                    catalogo.Update(Cita);
                    CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
                    CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
                    Application.Current.MainPage.Navigation.PopModalAsync();
                    VentanaCitas();
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        public void Eliminar()
        {
            Error = "";
            catalogo.Delete(Cita);
            CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
            CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
            Application.Current.MainPage.Navigation.PopModalAsync();
            VentanaCitas();
        }
        private void VentanaCitas()
        {
            Error = "";
            citasView = new CitasView() { BindingContext = this };

            Application.Current.MainPage.Navigation.PushModalAsync(citasView);
            CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
            CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
        }
        private void Cancelar()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
            CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
            CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
        }
        private void CancelarEdEl()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
            VentanaCitas();
            CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
            CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
        }
        public async void VentanaOpciones()
        {

            var accion = await Application.Current.MainPage.DisplayActionSheet(
                "Configuración KALU", "Cancelar", "Eliminar", $"¿Está seguro de que desea eliminar la cita de {Cita.Cliente} con el {Cita.Doctor}?");

            if (accion == "Eliminar")
            {
                Eliminar();
            }
            CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
            CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
        }

        private void VerCita(Citas c)
        {
            if (citaView == null)
                citaView = new VerCitaView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PopModalAsync();
            Application.Current.MainPage.Navigation.PushModalAsync(citaView);
            CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
            CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
            Cita = c;
        }

        public CitasViewModel()
        {
            CitasHoy = new ObservableCollection<Citas>(catalogo.GetCitasHoy());
            CitasREST = new ObservableCollection<Citas>(catalogo.GetCitas());
            CitasOld = new ObservableCollection<Citas>(catalogo.GetOldCitas());
            VerAgregarCommand = new Command(VerAgregar);
            AgregarCommand = new Command(Agregar);
            VerEditarCommand = new Command<Citas>(VerEditar);
            EditarCommand = new Command(Editar);
            EliminarCommand = new Command(VentanaOpciones);
            VerCitasCommand = new Command(VentanaCitas);
            CancelarCommand = new Command(Cancelar);
            CitaCommand = new Command<Citas>(VerCita);
            CancelarEdElCommand = new Command(CancelarEdEl);
        }
    }
}
