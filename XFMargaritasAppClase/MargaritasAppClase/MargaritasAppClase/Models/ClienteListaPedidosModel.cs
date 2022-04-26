using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class ClienteListaPedidosModel
    {
        public ClienteListaPedidosModel(string id_pedido,string id_cliente,string ult_cor_pedido,string fh_pedido,string fh_entrega,string comentarios,string audio,string tipopago,string tipoentrega,string latitud,string longitud,string direccion,string latitudped,string longitudped, string Entregador, string Telefono, string ID_Estado, string Estado, string NotiProceso, string NotiEntregado)
        {
            this.id_pedido = id_pedido;
            this.id_cliente = id_cliente;
            this.ult_cor_pedido = ult_cor_pedido;
            this.fh_pedido = fh_pedido;
            this.fh_entrega = fh_entrega;
            this.comentarios = comentarios;
            this.audio = audio;
            this.tipopago = tipopago;
            this.tipoentrega = tipoentrega;
            this.latitud = latitud;
            this.longitud = longitud;
            this.direccion = direccion;
            this.latitudped = latitudped;
            this.longitudped = longitudped;
            this.Entregador = Entregador;
            this.Telefono = Telefono;
            this.ID_Estado = ID_Estado;
            this.Estado = Estado;
            this.NotiProceso = NotiProceso;
            this.NotiEntregado = NotiEntregado;
        }

        public string id_pedido { get; set; }
        public string id_cliente { get; set; }
        public string ult_cor_pedido { get; set; }
        public string fh_pedido { get; set; }
        public string fh_entrega { get; set; }
        public string comentarios { get; set; }
        public string audio { get; set; }
        public string tipopago { get; set; }
        public string tipoentrega { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string direccion { get; set; }
        public string latitudped { get; set; }
        public string longitudped { get; set; }
        public string Entregador { get; set; }
        public string Telefono { get; set; }
        public string ID_Estado { get; set; }
        public string Estado { get; set; }
        public string NotiProceso { get; set; }
        public string NotiEntregado { get; set; }

    }
}
