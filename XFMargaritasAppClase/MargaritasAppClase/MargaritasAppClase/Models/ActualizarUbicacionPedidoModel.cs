using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class ActualizarUbicacionPedidoModel
    {
        public string Action { set; get; }
        public string ID_Cliente { set; get; }
        public string Correl { set; get; }
        public string LatitudPed { set; get; }
        public string LongitudPed { set; get; }
    }
}
