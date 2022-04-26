using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MargaritasAppClase.Models
{
    public class ClienteListaPedidosDetalleModel
    {
        public ClienteListaPedidosDetalleModel(string ID_Pedido, string ID_Cliente, string Ult_Cor_Pedido, string Cantidad, string Precio, string Foto, string Descripcion, ImageSource fotografia)
        {
            this.ID_Pedido = ID_Pedido;
            this.ID_Cliente = ID_Cliente;
            this.Ult_Cor_Pedido = Ult_Cor_Pedido;
            this.Cantidad = Cantidad;
            this.Precio = Precio;
            this.Foto = Foto;
            this.Descripcion = Descripcion;
            this.fotografia = fotografia;
        }

        public string ID_Pedido { set; get; }
        public string ID_Cliente { set; get; }
        public string Ult_Cor_Pedido { set; get; }
        public string Cantidad { set; get; }
        public string Precio { set; get; }
        public string Foto { set; get; }
        public string Descripcion { set; get; }
        public ImageSource fotografia { get; set; }
    }
}
