using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class SaveUbicacionModel
    {
        public string ID_Cliente { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Direccion { get; set; }
        public string Foto { get; set; }
        public string Nota { get; set; }
        public string Action { get; set; }

    }
}
