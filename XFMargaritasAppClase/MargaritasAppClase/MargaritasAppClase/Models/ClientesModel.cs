using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class ClientesModel
    {
        public string ID_Cliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string FechaNac { get; set; }
        public string FechaCrea { get; set; }
        public string Telefono { get; set; }
        public string Foto { get; set; }
        public string Estado { get; set; }
        public string TipoUsuario { get; set; }
    }
}
