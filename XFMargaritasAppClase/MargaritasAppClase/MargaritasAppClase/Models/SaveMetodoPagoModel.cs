using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class SaveMetodoPagoModel
    {
        public string ID_Cliente { get; set; }
        public string ID_FormaPago { get; set; }
        public string NumTcTd { get; set; }
        public string FechaExp { get; set; }
        public string Titular { get; set; }
        public string CCV { get; set; }
        public string Predeterminada { get; set; }
        public string Action { get; set; }

    }
}
