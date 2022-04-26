using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class CarritoEncabezadoModel
    {
        public string ID_Cliente { get; set; }
        public string Fecha { get; set; }
        public string SubTotal { get; set; }
        public string ISV { get; set; }
        public string Total { get; set; }
        public List<CarritoDetalleModel> DetalleCarrito { get; set; }

    }
}
