using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MargaritasAppClase.Models
{
    public class UbicacionModel
    {
        public string ID_Ubicacion { get; set; }
        public string ID_Cliente { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Direccion { get; set; }
        public string Foto { get; set; }
        public string Nota { get; set; }
        public string Action { get; set; }
        public ImageSource fotografia { get; set; }
    }
}
