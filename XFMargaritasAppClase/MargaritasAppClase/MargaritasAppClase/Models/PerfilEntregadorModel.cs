using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MargaritasAppClase.Models
{
    class PerfilEntregadorModel
    {
        public string ID_Cliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string FechaNac { get; set; }
        public string FechaCrea { get; set; }
        public string Telefono { get; set; }
        public ImageSource fotografia { get; set; }
        public string Foto { get; set; }
        public string Estado { get; set; }
        public string TipoUsuario { get; set; }
    }
}
