using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MargaritasAppClase.Models
{
    public class UbicacionesListModel
    {
        public UbicacionesListModel(string ID_Ubicacion, string ID_Cliente, string Latitud, string Longitud, string Direccion, string Foto, string Nota, ImageSource fotografia)
        {
            this.ID_Ubicacion = ID_Ubicacion;
            this.ID_Cliente = ID_Cliente;
            this.Latitud = Latitud;
            this.Longitud = Longitud;
            this.Direccion = Direccion;
            this.Foto = Foto;
            this.Nota = Nota;
            this.fotografia = fotografia;
        }

        public string ID_Ubicacion { get; set; }
        public string ID_Cliente { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Direccion { get; set; }
        public string Foto { get; set; }
        public string Nota { get; set; }
        public ImageSource fotografia { get; set; }

    }
}
