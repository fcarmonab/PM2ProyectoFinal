using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class OrdenEncabezadoModel
    {
        public string ID_Cliente { set; get; }
        public string ID_TipoPago { set; get; }
        public string ID_TipoEntrega { set; get; }
        public string FH_Pedido { set; get; }
        public string FH_Entrega { set; get; }
        public string ID_Ubicacion { set; get; }
        public string Comentarios { set; get; }
        public string Audio { set; get; }
        public string Estado { set; get; }
        public string ID_Entregador { set; get; }
        public List<OrdenDetalleModel> DetallePedido { set; get; }
    }
}
