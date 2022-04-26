using System;
using System.Collections.Generic;
using System.Text;

using MargaritasAppClase.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;



namespace MargaritasAppClase.Controller
{
    public class ProductsApiController
    {
        //Object objSitioGlobal = null;
        public async static Task<List<ProductsListModel>> ControllerObtenerListaProductos()
        {
            List<ProductsListModel> listaproductos = new List<ProductsListModel>();

            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync("https://webfacturacesar.000webhostapp.com/Margarita/methods/products/index.php");

                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = respuesta.Content.ReadAsStringAsync().Result.ToString();

                    dynamic dyn = JsonConvert.DeserializeObject(contenido);
                    byte[] newBytes = null;


                    if (contenido.Length > 28)
                    {

                        foreach (var item in dyn.items)
                        {
                            string img64 = item.Foto.ToString();
                            newBytes = Convert.FromBase64String(img64);
                            var stream = new MemoryStream(newBytes);

                            listaproductos.Add(new ProductsListModel(
                                            item.Id.ToString(), item.Descripcion.ToString(),
                                            item.Precio.ToString(),
                                            ImageSource.FromStream(() => stream),
                                            img64, item.Cantidad.ToString()
                                            ));
                        }
                    }                    

                }
            }
            return listaproductos;
        }

        public async static Task<ClientesModel> ControllerGetUser(string correo)
        {
            var clientesModel = new ClientesModel();

            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync("https://webfacturacesar.000webhostapp.com/Margarita/methods/cliente/" + correo);

                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = respuesta.Content.ReadAsStringAsync().Result;
                    clientesModel = JsonConvert.DeserializeObject<ClientesModel>(contenido);
                }
            }

            return await Task.FromResult(clientesModel);
        }


        public async static Task<List<CarritoListModel>> ControllerObtenerListaCarrito(string correo)
        {
            List<CarritoListModel> listacarrito = new List<CarritoListModel>();

            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync("https://webfacturacesar.000webhostapp.com/Margarita/methods/carrito/getItemsCarrito.php?mail=" + correo);

                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = respuesta.Content.ReadAsStringAsync().Result.ToString();

                    dynamic dyn = JsonConvert.DeserializeObject(contenido);
                    byte[] newBytes = null;


                    if (contenido.Length > 28)
                    {
                        if(dyn.msg == "Error")
                        {

                        }
                        else
                        {
                            foreach (var item in dyn.Carrito)
                            {
                                string idCarrito = item.ID_Carrito.ToString(), idCliente = item.ID_Cliente.ToString(), fecha = item.fecha.ToString(), subTotal = item.SubTotal.ToString(), isv = item.ISV.ToString(), total = item.Total.ToString();

                                foreach (var itemdetalle in item.DetalleCarrito)
                                {
                                    string idCarritoDetalle = itemdetalle["ID_CarritoDet"].ToString();
                                    string fkIdCarrito = itemdetalle.ID_Carrito.ToString();
                                    string codigoProducto = itemdetalle.ID_Producto.ToString();
                                    string descProducto = itemdetalle.desc_prod.ToString(), img64 = itemdetalle.foto.ToString(), cantidad = itemdetalle.Cantidad.ToString(), precio = itemdetalle.Precio.ToString(), totalDetalle = itemdetalle.Total.ToString();

                                    newBytes = Convert.FromBase64String(img64);
                                    var stream = new MemoryStream(newBytes);


                                    listacarrito.Add(new CarritoListModel(
                                                    idCarrito,
                                                    idCliente,
                                                    fecha,
                                                    subTotal,
                                                    isv,
                                                    total,
                                                    idCarritoDetalle,
                                                    fkIdCarrito,
                                                    codigoProducto,
                                                    descProducto,
                                                    img64,
                                                    ImageSource.FromStream(() => stream),
                                                    cantidad,
                                                    precio,
                                                    totalDetalle
                                                    ));

                                }

                            }
                        }
                    }
                }
            }
            return listacarrito;
        }


        public async static Task<List<UbicacionesListModel>> ControllerObtenerListaUbicaciones(string correo)
        {
            List<UbicacionesListModel> listaubicaciones = new List<UbicacionesListModel>();

            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync("https://webfacturacesar.000webhostapp.com/Margarita/methods/ubicaciones/?mail=" + correo);

                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = respuesta.Content.ReadAsStringAsync().Result.ToString();

                    dynamic dyn = JsonConvert.DeserializeObject(contenido);
                    byte[] newBytes = null;


                    if (contenido.Length > 28)
                    {
                        foreach (var item in dyn.Ubicaciones)
                        {
                            string img64 = item.Foto.ToString();
                            newBytes = Convert.FromBase64String(img64);
                            var stream = new MemoryStream(newBytes);

                            listaubicaciones.Add(new UbicacionesListModel(
                                            item.ID_Ubicacion.ToString(),
                                            item.ID_Cliente.ToString(),
                                            item.Latitud.ToString(),
                                            item.Longitud.ToString(),
                                            item.Direccion.ToString(),
                                            img64,
                                            item.Nota.ToString(),
                                            ImageSource.FromStream(() => stream)
                                            ));
                        }
                    }
                }
            }
            return listaubicaciones;
        }

        public async static Task<List<MetodosPagoListModel>> ControllerObtenerListaMetodosPagos(string correo)
        {
            List<MetodosPagoListModel> listametodospago = new List<MetodosPagoListModel>();

            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync("https://webfacturacesar.000webhostapp.com/Margarita/methods/payment/?mail=" + correo);

                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = respuesta.Content.ReadAsStringAsync().Result.ToString();

                    dynamic dyn = JsonConvert.DeserializeObject(contenido);

                    if (contenido.Length > 28)
                    {
                        foreach (var item in dyn.Formas)
                        {

                            listametodospago.Add(new MetodosPagoListModel(
                                            item.ID_Rel_Pago.ToString(),
                                            item.ID_Cliente.ToString(),
                                            item.ID_FormaPago.ToString(),
                                            item.Descripcion.ToString(),
                                            item.NumTcTd.ToString(),
                                            item.Titular.ToString(),
                                            item.Fecha_exp.ToString(),
                                            item.CCV.ToString(),
                                            item.Predeterminada.ToString()
                                            ));
                        }
                    }
                }
            }
            return listametodospago;
        }

        public async static Task<List<EntregadorListPedidosModel>> ControllerObtenerListaOrdenesEntregador(string correo)
        {
            List<EntregadorListPedidosModel> listaordenesentregador = new List<EntregadorListPedidosModel>();

            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/?mail="+correo+"&delivery");

                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = respuesta.Content.ReadAsStringAsync().Result.ToString();

                    dynamic dyn = JsonConvert.DeserializeObject(contenido);

                    if (contenido.Length > 28)
                    {
                        foreach (var item in dyn.Pedidos)
                        {
                            listaordenesentregador.Add(new EntregadorListPedidosModel(
                                            item.ID_Pedido.ToString(),
                                            item.ID_Cliente.ToString(), 
                                            item.Ult_Cor_Pedido.ToString(),
                                            item.FH_Pedido.ToString(),
                                            item.FH_Entrega.ToString(),
                                            item.Comentarios.ToString(),
                                            item.Audio.ToString(),
                                            item.TipoPago.ToString(),
                                            item.TipoEntrega.ToString(),
                                            item.Latitud.ToString(),
                                            item.Longitud.ToString(),
                                            item.Direccion.ToString(),
                                            item.LatitudPed.ToString(), 
                                            item.LongitudPed.ToString(),
                                            item.Entregador.ToString(),
                                            item.Telefono.ToString(),
                                            item.ID_Estado.ToString(),
                                            item.Estado.ToString()
                                            ));
                        }
                    }

                }
            }
            return listaordenesentregador;
        }

        public async static Task<List<EntregadorListPedidosModel>> ControllerObtenerListaOrdenesEntregadas(string correo)
        {
            List<EntregadorListPedidosModel> listaordenesentregador = new List<EntregadorListPedidosModel>();

            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/?mail=" + correo + "&delivery&hist=1");

                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = respuesta.Content.ReadAsStringAsync().Result.ToString();

                    dynamic dyn = JsonConvert.DeserializeObject(contenido);

                    if (contenido.Length > 28)
                    {
                        foreach (var item in dyn.Pedidos)
                        {
                            listaordenesentregador.Add(new EntregadorListPedidosModel(
                                            item.ID_Pedido.ToString(),
                                            item.ID_Cliente.ToString(),
                                            item.Ult_Cor_Pedido.ToString(),
                                            item.FH_Pedido.ToString(),
                                            item.FH_Entrega.ToString(),
                                            item.Comentarios.ToString(),
                                            item.Audio.ToString(),
                                            item.TipoPago.ToString(),
                                            item.TipoEntrega.ToString(),
                                            item.Latitud.ToString(),
                                            item.Longitud.ToString(),
                                            item.Direccion.ToString(),
                                            item.LatitudPed.ToString(),
                                            item.LongitudPed.ToString(),
                                            item.Entregador.ToString(),
                                            item.Telefono.ToString(),
                                            item.ID_Estado.ToString(),
                                            item.Estado.ToString()
                                            ));
                        }
                    }

                }
            }
            return listaordenesentregador;
        }

        public async static Task<List<ClienteListaPedidosModel>> ControllerObtenerListaOrdenesCliente(string correo)
        {
            List<ClienteListaPedidosModel> listaordenescliente = new List<ClienteListaPedidosModel>();

            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/?mail=" + correo);

                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = respuesta.Content.ReadAsStringAsync().Result.ToString();

                    dynamic dyn = JsonConvert.DeserializeObject(contenido);

                    if (contenido.Length > 28)
                    {
                        foreach (var item in dyn.Pedidos)
                        {
                            listaordenescliente.Add(new ClienteListaPedidosModel(
                                            item.ID_Pedido.ToString(),
                                            item.ID_Cliente.ToString(),
                                            item.Ult_Cor_Pedido.ToString(),
                                            item.FH_Pedido.ToString(),
                                            item.FH_Entrega.ToString(),
                                            item.Comentarios.ToString(),
                                            item.Audio.ToString(),
                                            item.TipoPago.ToString(),
                                            item.TipoEntrega.ToString(),
                                            item.Latitud.ToString(),
                                            item.Longitud.ToString(),
                                            item.Direccion.ToString(),
                                            item.LatitudPed.ToString(),
                                            item.LongitudPed.ToString(),
                                            item.Entregador.ToString(),
                                            item.Telefono.ToString(),
                                            item.ID_Estado.ToString(),
                                            item.Estado.ToString(),
                                            item.NotiProceso.ToString(),
                                            item.NotiEntregado.ToString()
                                            ));
                        }
                    }

                }
            }
            return listaordenescliente;
        }


        public async static Task<List<ClienteListaPedidosDetalleModel>> ControllerObtenerListaOrdenesClienteDetalle(string correo, string corel)
        {
            List<ClienteListaPedidosDetalleModel> listaordenesclientedetalle = new List<ClienteListaPedidosDetalleModel>();

            try
            {               

                using (HttpClient cliente = new HttpClient())
                {
                    var respuesta = await cliente.GetAsync("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/?mail=" + correo + "&corel=" + corel);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string contenido = respuesta.Content.ReadAsStringAsync().Result.ToString();

                        dynamic dyn = JsonConvert.DeserializeObject(contenido);
                        byte[] newBytes = null;

                        if (contenido.Length > 28)
                        {
                            foreach (var item in dyn.Pedidos)
                            {
                                string img64 = item.Foto.ToString();
                                newBytes = Convert.FromBase64String(img64);
                                var stream = new MemoryStream(newBytes);

                                listaordenesclientedetalle.Add(new ClienteListaPedidosDetalleModel(
                                                item.ID_Pedido.ToString(),
                                                item.ID_Cliente.ToString(),
                                                item.Ult_Cor_Pedido.ToString(),
                                                item.Cantidad.ToString(),
                                                item.Precio.ToString(),
                                                item.Foto.ToString(),
                                                item.Descripcion.ToString(),
                                                ImageSource.FromStream(() => stream)
                                                ));
                            }
                        }

                    }
                }
                return listaordenesclientedetalle;
            }
            catch(Exception ex)
            {
                return listaordenesclientedetalle;
            }
        }


    }
}
