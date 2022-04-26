using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class SaveOrdenModel
    {
        public string Action { set; get; }
        public List<OrdenEncabezadoModel> Pedidos { set; get; }
    }
}
