using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MargaritasAppClase.Models
{
    public class ProductsListModel
    {
        public ProductsListModel(string Id, string Descripcion, string Precio, ImageSource fotografia, string Foto, string Cantidad)
        {
            this.Id = Id;
            this.Descripcion = Descripcion;
            this.Precio = Precio;
            this.fotografia = fotografia;
            this.Foto = Foto;
            this.Cantidad = Cantidad;
        }

        public string Id { get; set; }
        public string Descripcion { get; set; }
        public string Precio { get; set; }
        public ImageSource fotografia { get; set; }
        public string Foto { get; set; }
        public string Cantidad { get; set; }
    }
}
