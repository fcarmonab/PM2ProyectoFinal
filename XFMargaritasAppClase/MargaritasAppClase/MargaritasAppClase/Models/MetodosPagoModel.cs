using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class MetodosPagoModel
    {
        public string ID_Rel_Pago { get; set; }
        public string ID_Cliente { get; set; }
        public string ID_FormaPago { get; set; }
        public string Descripcion { get; set; }
        public string NumTcTd { get; set; }
        public string Titular { get; set; }
        public string Fecha_exp { get; set; }
        public string CCV { get; set; }
        public string Predeterminada { get; set; }
    }
}
