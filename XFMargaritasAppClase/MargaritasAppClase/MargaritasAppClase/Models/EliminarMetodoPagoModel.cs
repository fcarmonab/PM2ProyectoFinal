using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class EliminarMetodoPagoModel
    {
        public string ID_Cliente { get; set; }
        public string ID_Rel_Pago { get; set; }
        public string Action { get; set; }
    }
}
